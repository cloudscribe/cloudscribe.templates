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
            Configuration = configuration;
            Environment = env;
            _log = logger;

            SslIsAvailable = Configuration.GetValue<bool>("AppSettings:UseSsl");
            #if (IdentityServer)
            DisableIdentityServer = Configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            #endif
        }

        private IConfiguration Configuration { get; set; }
        private IHostingEnvironment Environment { get; set; }
        private bool SslIsAvailable { get; set; }
        #if (IdentityServer)
        private bool DisableIdentityServer { get; set; }
        private bool didSetupIdServer = false;
        #endif
        private ILogger _log;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //// **** VERY IMPORTANT *****
            // This is a custom extension method in Config/DataProtection.cs
            // These settings require your review to correctly configur data protection for your environment
            services.SetupDataProtection(Configuration, Environment);
            
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
            services.SetupDataStorage(Configuration);
            
#if (IdentityServer)
            //*** Important ***
            // This is a custom extension method in Config/IdentityServerIntegration.cs
            // You should review this and understand what it does before deploying to production
            services.SetupIdentityServerIntegrationAndCORSPolicy(
                Configuration,
                Environment,
                _log,
                SslIsAvailable,
                DisableIdentityServer,
                out didSetupIdServer
                );

#endif
            //*** Important ***
            // This is a custom extension method in Config/CloudscribeFeatures.cs
            services.SetupCloudscribeFeatures(Configuration);

            //*** Important ***
            // This is a custom extension method in Config/Localization.cs
            services.SetupLocalization();

            //*** Important ***
            // This is a custom extension method in Config/RoutingAndMvc.cs
            services.SetupMvc(SslIsAvailable);


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
                    Environment.WebRootFileProvider
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
                    SslIsAvailable);

#if (IdentityServer)
            if (!DisableIdentityServer && didSetupIdServer)
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