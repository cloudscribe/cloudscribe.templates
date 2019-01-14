#if (IncludeHangfire)
using Hangfire;
#endif
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
#if (Webpack)
using Microsoft.AspNetCore.SpaServices.Webpack;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
#if (IncludeHangfire)
using WebApp.Config;
#endif


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
            #if (IncludeHangfire)
            _enableHangfireService = _configuration.GetValue<bool>("AppSettings:EnableHangfireService");
            _enableHangfireDashboard = _configuration.GetValue<bool>("AppSettings:EnableHangfireDashboard");
            #endif
        }

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly bool _sslIsAvailable;
        #if (IdentityServer)
        private readonly bool _disableIdentityServer;
        private bool _didSetupIdServer = false;
        #endif
        #if (IncludeHangfire)
        private readonly bool _enableHangfireService = true;
        private readonly bool _enableHangfireDashboard = true;
        #endif
        private readonly ILogger _log;

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
            #if (IncludeHangfire)
            var useHangfire = _enableHangfireService || _enableHangfireDashboard;
            services.SetupDataStorage(_configuration, useHangfire);
            #else
            services.SetupDataStorage(_configuration, false);
            #endif
            
            
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

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = cloudscribe.Core.Identity.SiteCookieConsent.NeedsConsent;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.ConsentCookie.Name = "cookieconsent_status";
            });

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
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IOptions<cloudscribe.Core.Models.MultiTenantOptions> multiTenantOptionsAccessor,
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                if(_sslIsAvailable)
                {
                    app.UseHsts();
                }
            }
            if(_sslIsAvailable)
            {
                app.UseHttpsRedirection();
            }
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
            app.UseCloudscribeCommonStaticFiles();
            app.UseCookiePolicy();

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

#if (IncludeHangfire)

            if (_enableHangfireDashboard)
            {
                app.UseHangfireDashboard("/tasks", new DashboardOptions
                {
                    Authorization = new[] { new HangFireAuthorizationFilter() }
                });
            }

            if (_enableHangfireService)
            {
                var options = new BackgroundJobServerOptions
                {
                    // This is the default value
                    //WorkerCount = Environment.ProcessorCount * 5
                    WorkerCount = 5
                };
                app.UseHangfireServer(options);

                GlobalConfiguration.Configuration.UseActivator(new cloudscribe.EmailQueue.HangfireIntegration.HangfireActivator(serviceProvider));

#if (IncludeEmailQueue)
                RecurringJob.RemoveIfExists("email-processor");
                RecurringJob.AddOrUpdate<cloudscribe.EmailQueue.Models.IEmailQueueProcessor>("email-processor", 
                    mp => mp.StartProcessing(), 
                    Cron.MinuteInterval(3)); //every 3 minutes
#endif

#if (Paywall)
                RecurringJob.RemoveIfExists("expired-membership-processor");
                RecurringJob.AddOrUpdate<cloudscribe.Membership.Models.IRoleRemovalTask>("expired-membership-processor", 
                    x => x.RemoveExpiredMembersFromGrantedRoles(), 
                    Cron.Daily(23)); //11pm

                RecurringJob.RemoveIfExists("membership-reminder-email-processor");
                RecurringJob.AddOrUpdate<cloudscribe.Membership.Models.ISendRemindersTask>("membership-reminder-email-processor", 
                    x => x.SendRenewalReminders(), 
                    Cron.Daily(7)); //7am
#endif


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