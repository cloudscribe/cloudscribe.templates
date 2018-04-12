using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {
#if (MultiTenantMode == 'FolderName')
        public static IRouteBuilder UseCustomRoutes(this IRouteBuilder routes, bool useFolders)
#else
        public static IRouteBuilder UseCustomRoutes(this IRouteBuilder routes)
#endif
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
                    defaults: new { action = "Index" },
                    constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );
#endif
#if (SimpleContentConfig == "c")
                //blog as default route
                routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "");
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
                , defaults: new { action = "Index" }
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
                , defaults: new { controller = "Home", action = "Index" }
                );
#endif
#if (SimpleContentConfig == "c")
            //blog as default route
            routes.AddBlogRoutesForSimpleContent("");
#endif
            
            return routes;
        }

        public static IServiceCollection SetupMvc(
            this IServiceCollection services,
            bool sslIsAvailable
            )
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto;
            });

            services.Configure<MvcOptions>(options =>
            {
                if (sslIsAvailable)
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

            return services;
        }

    }
}
