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
#if (!NoDb)
using Microsoft.EntityFrameworkCore;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
#if (KvpCustomRegistration)
using cloudscribe.UserProperties.Models;
using cloudscribe.UserProperties.Services;
#endif
#if (Webpack)
using Microsoft.AspNetCore.SpaServices.Webpack;
#endif

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;

            SslIsAvailable = Configuration.GetValue<bool>("AppSettings:UseSsl");
            #if (IdentityServer)
            DisableIdentityServer = Configuration.GetValue<bool>("AppSettings:DisableIdentityServer");
            IdentityServerX509CertificateThumbprintName = Configuration.GetValue<string>("AppSettings:IdentityServerX509CertificateThumbprintName");
            if(!DisableIdentityServer && Environment.IsProduction())
            {
                if (string.IsNullOrEmpty(IdentityServerX509CertificateThumbprintName))
                {
                    DisableIdentityServer = true;
                }
            }
            #endif
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; set; }
        public bool SslIsAvailable { get; set; }
        #if (IdentityServer)
        public bool DisableIdentityServer { get; set; }
        public string IdentityServerX509CertificateThumbprintName { get; set; }
        #endif

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // **** VERY IMPORTANT *****
            // https://www.cloudscribe.com/docs/configuring-data-protection
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
            #if (SimpleContentConfig != "z")
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
            if (!DisableIdentityServer)
            {
                if(Environment.IsProduction())
                {
                    services.AddIdentityServerConfiguredForCloudscribe()
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
                        .AddCloudscribeIdentityServerIntegrationMvc()
                        // *** IMPORTANT CHANGES NEEDED HERE *** 
                        // can't use .AddDeveloperSigningCredential in production it will throw an error
                        // https://identityserver4.readthedocs.io/en/dev/topics/crypto.html
                        // https://identityserver4.readthedocs.io/en/dev/topics/startup.html#refstartupkeymaterial
                        // you need to create an X.509 certificate (can be self signed)
                        // on your server and configure the thumbprint name in appsettings.json
                        // OR change this code to wire up a certificate differently
                         .AddSigningCredential(
                            IdentityServerX509CertificateThumbprintName, 
                            System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine,
                            NameType.Thumbprint
                            );
                }
                else
                {
                    services.AddIdentityServerConfiguredForCloudscribe()
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
                        .AddCloudscribeIdentityServerIntegrationMvc()
                        .AddDeveloperSigningCredential() // don't use this for production
                        ;
                }
            }
            
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                // add your IdentityServer client apps and apis to allow access to them
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:55347", "http://localhost:55347")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            #endif
            #if (ContactForm)
            services.AddCloudscribeSimpleContactForm(Configuration);
            #endif
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            #if (SimpleContentConfig != "z")
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();
            #endif
            services.AddCloudscribeCoreMvc(Configuration);
            #if (SimpleContentConfig != "z")
            services.AddCloudscribeCoreIntegrationForSimpleContent(Configuration);
            services.AddSimpleContentMvc(Configuration);
            services.AddMetaWeblogForSimpleContent(Configuration.GetSection("MetaWeblogApiOptions"));
            services.AddSimpleContentRssSyndiction();
            #endif
            
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

            services.Configure<MvcOptions>(options =>
            {
                if (SslIsAvailable)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                #if (SimpleContentConfig == 'a' || SimpleContentConfig == 'b')
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
                    options.AddCloudscribeCommonEmbeddedViews();
                    options.AddCloudscribeNavigationBootstrap3Views();
                    options.AddCloudscribeCoreBootstrap3Views();
                    #if (SimpleContentConfig != "z")
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
            if (!DisableIdentityServer)
            {
               app.UseIdentityServer();
            }
            #endif
            #if (MultiTenantMode == 'FolderName')
            UseMvc(app, multiTenantOptions.Mode == cloudscribe.Core.Models.MultiTenantMode.FolderName);
            #else
            UseMvc(app);
            #endif
            
        }
#if (MultiTenantMode == 'FolderName')
        private void UseMvc(IApplicationBuilder app, bool useFolders)
#else
        private void UseMvc(IApplicationBuilder app)
#endif
        {
            app.UseMvc(routes =>
            {
                #if (SimpleContentConfig != "z")
                #if (SimpleContentConfig != "c")
                #if (MultiTenantMode == 'FolderName')
                if (useFolders)
                {
                    routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
                }
                #endif
                
                routes.AddBlogRoutesForSimpleContent();
                #endif
                routes.AddSimpleContentStaticResourceRoutes();
                #endif
                routes.AddCloudscribeFileManagerRoutes();
                #if (MultiTenantMode == 'FolderName')
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

                    
                    #if (SimpleContentConfig == "a" || SimpleContentConfig == "b")
                    routes.MapRoute(
                            name: "foldersitemap",
                            template: "{sitefolder}/sitemap"
                            , defaults: new { controller = "Page", action = "SiteMap" }
                            , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                            );
                    #endif
                    #if (SimpleContentConfig == "c")
                    // you can easily add pages by uncommenting this and uncommenting the coresponding node in navigation.xml
                    //routes.AddCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "NONROOTPAGESEGMENT");
                    #endif
                    #if (SimpleContentConfig == "a" || SimpleContentConfig == "c")
                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: null,
                        constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );
                    #endif
                    #if (SimpleContentConfig == "a")
                    routes.AddDefaultPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
                    #endif
                    #if (SimpleContentConfig == "b")
                    routes.AddCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "NONROOTPAGESEGMENT");
                    #endif

                    #if (SimpleContentConfig == "z" || SimpleContentConfig == "b" || SimpleContentConfig == "d")
                    routes.MapRoute(
                        name: "folderdefault",
                        template: "{sitefolder}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" },
                        constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );
                    #endif

                    #if (SimpleContentConfig == "c")
                    //blog as default route
                    routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(),"");
                    #endif
                }
                #endif

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

                 #if (SimpleContentConfig == "a" || SimpleContentConfig == "b")
                routes.MapRoute(
                    name: "sitemap",
                    template: "sitemap"
                    , defaults: new { controller = "Page", action = "SiteMap" }
                    );
                #endif
                #if (SimpleContentConfig == "c")
                // you can easily add pages by uncommenting this and uncommenting the coresponding node in navigation.xml
                //routes.AddCustomPageRouteForSimpleContent("NONROOTPAGESEGMENT");
                #endif
                #if (SimpleContentConfig == "a" || SimpleContentConfig == "c")
                routes.MapRoute(
                    name: "def",
                    template: "{controller}/{action}"
                    //,defaults: new { controller = "Home", action = "Index" }
                    );
                #endif
                #if (SimpleContentConfig == "a")
                routes.AddDefaultPageRouteForSimpleContent();
                #endif
                #if (SimpleContentConfig == "b")
                routes.AddCustomPageRouteForSimpleContent("NONROOTPAGESEGMENT");
                #endif
                

                #if (SimpleContentConfig == "z" || SimpleContentConfig == "b" || SimpleContentConfig == "d")
                routes.MapRoute(
                    name: "def",
                    template: "{controller}/{action}"
                    ,defaults: new { controller = "Home", action = "Index" }
                    );
                #endif

                #if (SimpleContentConfig == "c")
                //blog as default route
                routes.AddBlogRoutesForSimpleContent("");
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
                
                #if (SimpleContentConfig != "z")
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