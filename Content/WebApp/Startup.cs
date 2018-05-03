using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
#if (Webpack)
using Microsoft.AspNetCore.SpaServices.Webpack;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;


namespace WebApp
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration, 
            IHostingEnvironment env,
            ILogger<Startup> logger
            )
        {
            _configuration = configuration;
            _environment = env;
            _log = logger;

            _sslIsAvailable = _configuration.GetValue<bool>("AppSettings:UseSsl");
            #if (IdentityServer)
            _disableIdentityServer = _configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            #endif
        }

        private IConfiguration _configuration { get; set; }
        private IHostingEnvironment _environment { get; set; }
        private bool _sslIsAvailable { get; set; }
        #if (IdentityServer)
        private bool _disableIdentityServer { get; set; }
        private bool _didSetupIdServer = false;
        #endif
        private ILogger _log;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
            services.SetupDataStorage(_configuration);
            
#if (IdentityServer)
            //*** Important ***
            // This is a custom extension method in Config/IdentityServerIntegration.cs
            // You should review this and understand what it does before deploying to production
            services.SetupIdentityServerIntegrationAndCORSPolicy(
                _configuration,
                _environment,
                _log,
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
            services.SetupLocalization();

            //*** Important ***
            // This is a custom extension method in Config/RoutingAndMvc.cs
            services.SetupMvc(_sslIsAvailable);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                #if (Webpack)
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                    #if (IncludeReact)
                    ,ReactHotModuleReplacement = true
                    #endif
                });
                #endif
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/oops/error");
            }

            app.UseForwardedHeaders();
            #if (Webpack)
            // we are pre-gzipping js and css from webpack
            // this allows us to map the requests for .min.js to .min.js.gz and .min.css to .min.css.gz if the file exists
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = GzipMappingFileProvider.OnPrepareResponse,
                FileProvider = new GzipMappingFileProvider(
                    loggerFactory,
                    true,
                    _environment.WebRootFileProvider
                    )
            });
            #else
            app.UseStaticFiles();
            #endif

            //app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);
            #if (IdentityServer)
            app.UseCors("default"); //use Cors with policy named default, defined above
            #endif

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore(
                    loggerFactory,
                    multiTenantOptions,
                    _sslIsAvailable);

#if (IdentityServer)
            if (!_disableIdentityServer && _didSetupIdServer)
            {
                try
                {
                    app.UseIdentityServer();
                }
                catch(Exception ex)
                {
                    _log.LogError($"failed to setup identityserver4 {ex.Message} {ex.StackTrace}");
                }
            }
#endif

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
   
        }

        
        
    }
}