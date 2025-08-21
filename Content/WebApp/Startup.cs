using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
#if (Webpack)
using Microsoft.AspNetCore.SpaServices.Webpack;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace WebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration, 
            IWebHostEnvironment env
            )
        {
            _configuration = configuration;
            _environment = env;
            
            _sslIsAvailable = _configuration.GetValue<bool>("AppSettings:UseSsl");
            #if (IdentityServer)
            _disableIdentityServer = _configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            #endif
        }

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly bool _sslIsAvailable;
        #if (IdentityServer)
        private readonly bool _disableIdentityServer;
        private bool _didSetupIdServer = false;
        #endif
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();

            //// **** VERY IMPORTANT *****
            // This is a custom extension method in Config/DataProtection.cs
            // These settings require your review to correctly configur data protection for your environment
            services.SetupDataProtection(_configuration, _environment);
            
            services.AddAuthorization(options =>
            {
                //https://docs.asp.net/en/latest/security/authorization/policies.html
                //** IMPORTANT ***
                //This is a custom extension method in Config/Authorization.cs
                //That is where you can review or customize or add additional authorization policies
                options.SetupAuthorizationPolicies();

            });

            //// **** IMPORTANT *****
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupDataStorage(_configuration, _environment);
            
            
#if (IdentityServer)
            //*** Important ***
            // This is a custom extension method in Config/IdentityServerIntegration.cs
            // You should review this and understand what it does before deploying to production
            services.SetupIdentityServerIntegrationAndCORSPolicy(
                _configuration,
                _environment,
                _sslIsAvailable,
                _disableIdentityServer,
                out _didSetupIdServer
                );

#endif
            //*** Important ***
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupCloudscribeFeatures(_configuration);

            //*** Important ***
            // This is a custom extension method in Config/Localization.cs
            services.SetupLocalization(_configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = cloudscribe.Core.Identity.SiteCookieConsent.NeedsConsent;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                options.ConsentCookie.Name = "cookieconsent_status";
                options.Secure = CookieSecurePolicy.Always;
            });

            if (_sslIsAvailable)
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(180);
                });
            }

            services.Configure<Microsoft.AspNetCore.Mvc.CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            //*** Important ***
            // This is a custom extension method in Config/RoutingAndMvc.cs
            services.SetupMvc(_sslIsAvailable);

            if(!_environment.IsDevelopment())
            {
                var httpsPort = _configuration.GetValue<int>("AppSettings:HttpsPort");
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = httpsPort;
                });
            }


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IServiceProvider serviceProvider,
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            // When running behind a proxy, the request to the proxy may be made via https,
            // but the proxy may be configured to talk to cloudscribe by http only. In that case,
            // we want our httpContext to contain the correct client IP address (rather than 127.0.0.1)
            // and the correct protocol (https).
            // See: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-apache?view=aspnetcore-3.1#configure-apache
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/oops/error");
                if(_sslIsAvailable)
                {
                    app.UseHsts();
                }
            }
            if(_sslIsAvailable)
            {
                app.UseHttpsRedirection();
            }
            
            app.UseStaticFiles();
            app.UseCloudscribeCommonStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);
            #if (IdentityServer)
            app.UseCors("default"); //use Cors with policy named default, defined above
            #endif

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore();

#if (IdentityServer)
            if (!_disableIdentityServer && _didSetupIdServer)
            {
                app.UseIdentityServer();  
            }
#endif

#pragma warning disable MVC1005 // Cannot use UseMvc with Endpoint Routing.
// workaround for 
//https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/466
 app.UseMvc(routes =>
            {
#if (MultiTenantMode == 'FolderName')
                var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
                //*** IMPORTANT ***
                // this is in Config/RoutingAndMvc.cs
                // you can change or add routes there
                routes.UseCustomRoutes(useFolders);
#else
                routes.UseCustomRoutes();
#endif
            });
#pragma warning restore MVC1005 // Cannot use UseMvc with Endpoint Routing.

//             app.UseEndpoints(endpoints =>
//             {
// #if (MultiTenantMode == 'FolderName')
//                 var useFolders = multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName;
//                 //*** IMPORTANT ***
//                 // this is in Config/RoutingAndMvc.cs
//                 // you can change or add routes there
//                 endpoints.UseCustomRoutes(useFolders);
// #else
//                 endpoints.UseCustomRoutes();
// #endif
//             });
   
        }

        
        
    }
}