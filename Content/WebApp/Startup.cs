using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
#if (KvpCustomRegistration)
using cloudscribe.UserProperties.Models;
using cloudscribe.UserProperties.Services;
#endif

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; set; }
        public bool SslIsAvailable { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // **** VERY IMPORTANT *****
            // data protection keys are used to encrypt the auth token in the cookie
            // and also to encrypt social auth secrets and smtp password in the data storage
            // therefore we need keys to be persistent in order to be able to decrypt
            // if you move an app to different hosting and the keys change then you would have
            // to update those settings again from the Administration UI

            // for IIS hosting you should use a powershell script to create a keyring in the registry
            // per application pool and use a different application pool per app
            // https://docs.microsoft.com/en-us/aspnet/core/publishing/iis#data-protection
            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?tabs=aspnetcore2x
            if(Environment.IsProduction())
            {
                // If using Azure for production the uri with sas token could be stored in azure as environment variable or using key vault
                // but the keys go in azure blob storage per docs https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers
                //var dpKeysUrl = Configuration["AppSettings:DataProtectionKeysBlobStorageUrl"];
                services.AddDataProtection()
                    //.PersistKeysToAzureBlobStorage(new Uri(dpKeysUrl))
                    ;
            }
            else
            {
                // dp_Keys folder should be added to .gitignore so the keys don't go into source control
                // ie add a line with: **/dp_keys/**
                // to your .gitignore file
                string pathToCryptoKeys = Path.Combine(Environment.ContentRootPath, "dp_keys");
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(pathToCryptoKeys))
                    ;
            }

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            services.AddMemoryCache();

            //services.AddSession();

            ConfigureAuthPolicy(services);

            services.AddOptions();

            #if (KvpCustomRegistration)
            services.Configure<ProfilePropertySetContainer>(Configuration.GetSection("ProfilePropertySetContainer"));
            services.AddCloudscribeKvpUserProperties();
            #endif

            #if (!NoDb)
            var connectionString = Configuration.GetConnectionString("EntityFrameworkConnection");

            #if (MSSQL)
            services.AddCloudscribeCoreEFStorageMSSQL(connectionString);
            #endif
            #if (MySql)
            services.AddCloudscribeCoreEFStorageMySql(connectionString);
            #endif
            #if (pgsql)
            services.AddCloudscribeCoreEFStoragePostgreSql(connectionString);
            #endif
            #else
            services.AddCloudscribeCoreNoDbStorage();
            #endif
            #if (KvpCustomRegistration)  
            #if (NoDb)
            services.AddCloudscribeKvpNoDbStorage();
            #endif
            #if (MSSQL)
            services.AddCloudscribeKvpEFStorageMSSQL(connectionString);
            #endif
            #if (MySql)
            services.AddCloudscribeKvpEFStorageMySql(connectionString);
            #endif
            #if (pgsql)
            services.AddCloudscribeKvpEFStoragePostgreSql(connectionString);
            #endif
            #endif
            #if (Logging)
            #if (NoDb)
            services.AddCloudscribeLoggingNoDbStorage(Configuration);
            #endif
            #if (MSSQL)
            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);
            #endif
            #if (MySql)
            services.AddCloudscribeLoggingEFStorageMySQL(connectionString);
            #endif
            #if (pgsql)
            services.AddCloudscribeLoggingEFStoragePostgreSql(connectionString);
            #endif
            services.AddCloudscribeLogging();
            #endif
            #if (SimpleContent)
            #if (NoDb)
            services.AddNoDbStorageForSimpleContent();
            #endif
            #if (MSSQL)
            services.AddCloudscribeSimpleContentEFStorageMSSQL(connectionString);
            #endif
            #if (MySql)
            services.AddCloudscribeSimpleContentEFStorageMySQL(connectionString);
            #endif
            #if (pgsql)
            services.AddCloudscribeSimpleContentEFStoragePostgreSql(connectionString);
            #endif
            #endif

            #if (IdentityServer)
            if(Environment.IsProduction())
            {
                services.AddIdentityServer()
            #if (NoDb)
                    .AddCloudscribeCoreNoDbIdentityServerStorage()
            #endif
            #if (MSSQL)
                    .AddCloudscribeCoreEFIdentityServerStorageMSSQL(connectionString)
            #endif
            #if (MySql)
                    .AddCloudscribeCoreEFIdentityServerStorageMySql(connectionString)
            #endif
            #if (pgsql)
                    .AddCloudscribeCoreEFIdentityServerStoragePostgreSql(connectionString)
            #endif
                    .AddCloudscribeIdentityServerIntegration()
                    // *** IMPORTANT CHANGES NEEDED HERE *** 
                    // don't use AddDeveloperSigningCredential in production
                    // https://identityserver4.readthedocs.io/en/dev/topics/crypto.html
                    //.AddSigningCredential(cert) // create a certificate for use in production
                    .AddDeveloperSigningCredential() // don't use this for production
                    ;
            }
            else
            {
                services.AddIdentityServer()
            #if (NoDb)
                    .AddCloudscribeCoreNoDbIdentityServerStorage()
            #endif
            #if (MSSQL)
                    .AddCloudscribeCoreEFIdentityServerStorageMSSQL(connectionString)
            #endif
            #if (MySql)
                    .AddCloudscribeCoreEFIdentityServerStorageMySql(connectionString)
            #endif
            #if (pgsql)
                    .AddCloudscribeCoreEFIdentityServerStoragePostgreSql(connectionString)
            #endif
                    .AddCloudscribeIdentityServerIntegration()
                    .AddDeveloperSigningCredential() // don't use this for production
                    ;
            }
            #endif
            #if (ContactForm)
            services.AddCloudscribeSimpleContactForm(Configuration);
            #endif
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            #if (SimpleContent)
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();
            #endif
            services.AddCloudscribeCore(Configuration);
            #if (SimpleContent)
            services.AddCloudscribeCoreIntegrationForSimpleContent();
            services.AddSimpleContent(Configuration);
            services.AddMetaWeblogForSimpleContent(Configuration.GetSection("MetaWeblogApiOptions"));
            services.AddSimpleContentRssSyndiction();
            #endif
            services.AddCloudscribeFileManagerIntegration(Configuration);

            // optional but recommended if you need localization 
            // uncomment to use cloudscribe.Web.localization https://github.com/joeaudette/cloudscribe.Web.Localization
            //services.Configure<GlobalResourceOptions>(Configuration.GetSection("GlobalResourceOptions"));
            //services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();

            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    //new CultureInfo("en-GB"),
                    //new CultureInfo("fr-FR"),
                    //new CultureInfo("fr"),
                };

                // State what the default culture for your application is. This will be used if no specific culture
                // can be determined for a given request.
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting numbers, dates, etc.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
                options.SupportedUICultures = supportedCultures;

                // You can change which providers are configured to determine the culture for requests, or even add a custom
                // provider with your own logic. The providers will be asked in order to provide a culture for each request,
                // and the first to provide a non-null result that is in the configured supported cultures list will be used.
                // By default, the following built-in providers are configured:
                // - QueryStringRequestCultureProvider, sets culture via "culture" and "ui-culture" query string values, useful for testing
                // - CookieRequestCultureProvider, sets culture via "ASPNET_CULTURE" cookie
                // - AcceptLanguageHeaderRequestCultureProvider, sets culture via the "Accept-Language" request header
                //options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                //{
                //  // My custom request culture logic
                //  return new ProviderCultureResult("en");
                //}));
            });

            SslIsAvailable = Configuration.GetValue<bool>("AppSettings:UseSsl");
            services.Configure<MvcOptions>(options =>
            {
                if (SslIsAvailable)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                #if (SimpleContent)
                options.CacheProfiles.Add("SiteMapCacheProfile",
                     new CacheProfile
                     {
                         Duration = 30
                     });

                options.CacheProfiles.Add("RssCacheProfile",
                     new CacheProfile
                     {
                         Duration = 100
                     });
                #endif
            });

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    options.AddCloudscribeViewLocationFormats();

                    options.AddCloudscribeCommonEmbeddedViews();
                    options.AddCloudscribeNavigationBootstrap3Views();
                    options.AddCloudscribeCoreBootstrap3Views();
                    #if (SimpleContent)
                    options.AddCloudscribeSimpleContentBootstrap3Views();
                    #endif
                    options.AddCloudscribeFileManagerBootstrap3Views();
                    #if (Logging)
                    options.AddCloudscribeLoggingBootstrap3Views();
                    #endif
                    #if (IdentityServer)
                    options.AddCloudscribeCoreIdentityServerIntegrationBootstrap3Views();
                    #endif
                    #if (ContactForm)
                    options.AddCloudscribeSimpleContactFormViews();
                    #endif

                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                });
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
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/oops/error");
            }

            app.UseForwardedHeaders();
            app.UseStaticFiles();
            //app.UseSession();

            app.UseRequestLocalization(localizationOptionsAccessor.Value);

            var multiTenantOptions = multiTenantOptionsAccessor.Value;

            app.UseCloudscribeCore(
                    loggerFactory,
                    multiTenantOptions,
                    SslIsAvailable);

            #if (IdentityServer)
            app.UseIdentityServer();
            #endif

            UseMvc(app, multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName);
            
        }

        private void UseMvc(IApplicationBuilder app, bool useFolders)
        {
            app.UseMvc(routes =>
            {
                #if (SimpleContent)
                if (useFolders)
                {
                    routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
                }
                
                routes.AddBlogRoutesForSimpleContent();
                routes.AddSimpleContentStaticResourceRoutes();
                #endif
                routes.AddCloudscribeFileManagerRoutes();

                if (useFolders)
                {
                    routes.MapRoute(
                       name: "foldererrorhandler",
                       template: "{sitefolder}/oops/error/{statusCode?}",
                       defaults: new { controller = "Oops", action = "Error" },
                       constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );

                    #if (ContactForm)
                    routes.MapRoute(
                        name: "foldercontact",
                        template: "{sitefolder}/contact",
                        defaults: new { controller = "Contact", action = "Index" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );
                    #endif

                    #if (SimpleContent)
                    routes.MapRoute(
                            name: "foldersitemap",
                            template: "{sitefolder}/sitemap"
                            , defaults: new { controller = "Page", action = "SiteMap" }
                            , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                            );

                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: null,
                        constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );
                    
                    routes.AddDefaultPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
                    #else
                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" },
                        constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );
                    #endif
                }

                routes.MapRoute(
                    name: "errorhandler",
                    template: "oops/error/{statusCode?}",
                    defaults: new { controller = "Oops", action = "Error" }
                    );

                #if (ContactForm)
                routes.MapRoute(
                    name: "contact",
                    template: "contact",
                    defaults: new { controller = "Contact", action = "Index" }
                    );
                #endif

                #if (SimpleContent)
                routes.MapRoute(
                    name: "sitemap",
                    template: "sitemap"
                    , defaults: new { controller = "Page", action = "SiteMap" }
                    );


                routes.MapRoute(
                    name: "def",
                    template: "{controller}/{action}"
                    //,defaults: new { controller = "Home", action = "Index" }
                    );

                routes.AddDefaultPageRouteForSimpleContent();
                #else
                routes.MapRoute(
                    name: "def",
                    template: "{controller}/{action}"
                    ,defaults: new { controller = "Home", action = "Index" }
                    );
                #endif

            });
        }

        private void ConfigureAuthPolicy(IServiceCollection services)
        {
            //https://docs.asp.net/en/latest/security/authorization/policies.html

            services.AddAuthorization(options =>
            {
                options.AddCloudscribeCoreDefaultPolicies();
                #if (Logging)
                options.AddCloudscribeLoggingDefaultPolicy();
                #endif
                
                #if (SimpleContent)
                options.AddCloudscribeCoreSimpleContentIntegrationDefaultPolicies();
                // this is what the above extension adds
                //options.AddPolicy(
                //    "BlogEditPolicy",
                //    authBuilder =>
                //    {
                //        //authBuilder.RequireClaim("blogId");
                //        authBuilder.RequireRole("Administrators");
                //    }
                // );

                //options.AddPolicy(
                //    "PageEditPolicy",
                //    authBuilder =>
                //    {
                //        authBuilder.RequireRole("Administrators");
                //    });
                #endif

                options.AddPolicy(
                    "FileManagerPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators", "Content Administrators");
                    });

                options.AddPolicy(
                    "FileManagerDeletePolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators", "Content Administrators");
                    });

                #if (IdentityServer)
                options.AddPolicy(
                    "IdentityServerAdminPolicy",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("Administrators");
                    });
                #endif

                // add other policies here 

            });

        }

        
    }
}