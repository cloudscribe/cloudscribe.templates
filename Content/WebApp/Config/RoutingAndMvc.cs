﻿using cloudscribe.Web.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class RoutingAndMvc
    {

        /// this traditional mvc routing works around a bug in endpoint routing
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
                routes.AddCultureBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true));
                routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
            }
#endif          
            routes.AddCultureBlogRoutesForSimpleContent(new CultureSegmentRouteConstraint());    
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

                routes.MapRoute(
                      name: "apifoldersitemap-localized",
                      template: "{sitefolder}/{culture}/api/sitemap"
                      , defaults: new { controller = "FolderSiteMap", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapRoute(
                       name: "apifoldersitemap",
                       template: "{sitefolder}/api/sitemap"
                       , defaults: new { controller = "FolderSiteMap", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

#if (ContactForm)

                routes.MapRoute(
                    name: "foldercontact-localized",
                    template: "{sitefolder}/{culture}/contact",
                    defaults: new { controller = "Contact", action = "Index" }
                    , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                    );

                routes.MapRoute(
                    name: "foldercontact",
                    template: "{sitefolder}/contact",
                    defaults: new { controller = "Contact", action = "Index" }
                    , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );
#endif
#if (SimpleContentConfig == "a" || SimpleContentConfig == "b")
                 routes.MapRoute(
                        name: "foldersitemap-localized",
                        template: "{sitefolder}/{culture}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                        );
                
                routes.MapRoute(
                        name: "foldersitemap",
                        template: "{sitefolder}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                 routes.MapRoute(
                      name: "apifoldermetaweblog-localized",
                      template: "{sitefolder}/{culture}/api/metaweblog"
                      , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapRoute(
                       name: "apifoldermetaweblog",
                       template: "{sitefolder}/api/metaweblog"
                       , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapRoute(
                       name: "apifolderrss",
                       template: "{sitefolder}/api/rss"
                       , defaults: new { controller = "FolderRss", action = "Index" }
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
                    defaults: new { action = "Index" },
                    constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );
#endif
#if (SimpleContentConfig == "a")
                routes.AddCulturePageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true));
                routes.AddDefaultPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
#endif
#if (SimpleContentConfig == "b")
                routes.AddCultureCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true), "NONROOTPAGESEGMENT");
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
#if (Forum)
            routes.AddForumRoutes(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
#endif            

#if (ContactForm)
            routes.MapRoute(
                name: "contact",
                template: "contact",
                defaults: new { controller = "Contact", action = "Index" }
                );
#endif

#if (SimpleContentConfig == "a" || SimpleContentConfig == "b")
            routes.MapRoute(
                       name: "api-sitemap-culture",
                       template: "{culture}/api/sitemap"
                       , defaults: new { controller = "CultureSiteMap", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                       name: "api-rss-culture",
                       template: "{culture}/api/rss"
                       , defaults: new { controller = "CultureRss", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                       name: "api-metaweblog-culture",
                       template: "{culture}/api/metaweblog"
                       , defaults: new { controller = "CultureMetaweblog", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapRoute(
                name: "sitemap-localized",
                template: "{culture}/sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                , constraints: new { culture = new CultureSegmentRouteConstraint() }
                );

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
                    name: "default-localized",
                    template: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );
                    
            routes.MapRoute(
                name: "def",
                template: "{controller}/{action}"
                , defaults: new { action = "Index" }
                );
#endif
#if (SimpleContentConfig == "a")
            routes.AddCulturePageRouteForSimpleContent(new CultureSegmentRouteConstraint());
            routes.AddDefaultPageRouteForSimpleContent();
#endif
#if (SimpleContentConfig == "b")
            routes.AddCultureCustomPageRouteForSimpleContent(new CultureSegmentRouteConstraint(),"NONROOTPAGESEGMENT");
            routes.AddCustomPageRouteForSimpleContent("NONROOTPAGESEGMENT");
#endif

#if (SimpleContentConfig == "z" || SimpleContentConfig == "b" || SimpleContentConfig == "d")
            routes.MapRoute(
                    name: "default-localized",
                    template: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

            routes.MapRoute(
                name: "def",
                template: "{controller}/{action}"
                , defaults: new { controller = "Home", action = "Index" }
                );
#endif
#if (SimpleContentConfig == "c")
            //blog as default route
            routes.AddCultureBlogRoutesForSimpleContent(new CultureSegmentRouteConstraint(), "");
            routes.AddBlogRoutesForSimpleContent("");
#endif
            
            return routes;
        }











        // this new endpoint routing has bugs that breaks folder and culture route constraints, this code could be used later after aspnetcore team fixes the bug
        // https://github.com/aspnet/AspNetCore/issues/14877
#if (MultiTenantMode == 'FolderName')
        public static IEndpointRouteBuilder UseCustomRoutes(this IEndpointRouteBuilder routes, bool useFolders)
#else
        public static IEndpointRouteBuilder UseCustomRoutes(this IEndpointRouteBuilder routes)
#endif
        {
#if (SimpleContentConfig != "z")
#if (SimpleContentConfig != "c")
#if (MultiTenantMode == 'FolderName')
            if (useFolders)
            {
                routes.AddCultureBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true));
                routes.AddBlogRoutesForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
            }
#endif          
            routes.AddCultureBlogRoutesForSimpleContent(new CultureSegmentRouteConstraint());    
            routes.AddBlogRoutesForSimpleContent();
#endif
            routes.AddSimpleContentStaticResourceRoutes();
#endif
            routes.AddCloudscribeFileManagerRoutes();
#if (MultiTenantMode == 'FolderName')
            if (useFolders)
            {
                routes.MapControllerRoute(
                    name: "foldererrorhandler",
                    pattern: "{sitefolder}/oops/error/{statusCode?}",
                    defaults: new { controller = "Oops", action = "Error" },
                    constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                );

                routes.MapControllerRoute(
                      name: "apifoldersitemap-localized",
                      pattern: "{sitefolder}/{culture}/api/sitemap"
                      , defaults: new { controller = "FolderSiteMap", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapControllerRoute(
                       name: "apifoldersitemap",
                       pattern: "{sitefolder}/api/sitemap"
                       , defaults: new { controller = "FolderSiteMap", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

#if (ContactForm)

                routes.MapControllerRoute(
                    name: "foldercontact-localized",
                    pattern: "{sitefolder}/{culture}/contact",
                    defaults: new { controller = "Contact", action = "Index" }
                    , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                    );

                routes.MapControllerRoute(
                    name: "foldercontact",
                    pattern: "{sitefolder}/contact",
                    defaults: new { controller = "Contact", action = "Index" }
                    , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );
#endif
#if (SimpleContentConfig == "a" || SimpleContentConfig == "b")
                 routes.MapControllerRoute(
                        name: "foldersitemap-localized",
                        pattern: "{sitefolder}/{culture}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                        );
                
                routes.MapControllerRoute(
                        name: "foldersitemap",
                        pattern: "{sitefolder}/sitemap"
                        , defaults: new { controller = "Page", action = "SiteMap" }
                        , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                        );

                 routes.MapControllerRoute(
                      name: "apifoldermetaweblog-localized",
                      pattern: "{sitefolder}/{culture}/api/metaweblog"
                      , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                      , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), culture = new CultureSegmentRouteConstraint(true) }
                      );

                routes.MapControllerRoute(
                       name: "apifoldermetaweblog",
                       pattern: "{sitefolder}/api/metaweblog"
                       , defaults: new { controller = "FolderMetaweblog", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

                routes.MapControllerRoute(
                       name: "apifolderrss",
                       pattern: "{sitefolder}/api/rss"
                       , defaults: new { controller = "FolderRss", action = "Index" }
                       , constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                       );

#endif
#if (SimpleContentConfig == "c")
                // you can easily add pages by uncommenting this and uncommenting the coresponding node in navigation.xml
                //routes.AddCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "NONROOTPAGESEGMENT");
#endif
#if (SimpleContentConfig == "a" || SimpleContentConfig == "c")
                routes.MapControllerRoute(
                    name: "folderdefault",
                    pattern: "{sitefolder}/{controller}/{action}/{id?}",
                    defaults: null,
                    constraints: new { name = new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint() }
                    );
#endif
#if (SimpleContentConfig == "a")
                routes.AddCulturePageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true));
                routes.AddDefaultPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
#endif
#if (SimpleContentConfig == "b")
                routes.AddCultureCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), new CultureSegmentRouteConstraint(true), "NONROOTPAGESEGMENT");
                routes.AddCustomPageRouteForSimpleContent(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint(), "NONROOTPAGESEGMENT");
#endif
#if (SimpleContentConfig == "z" || SimpleContentConfig == "b" || SimpleContentConfig == "d")
                routes.MapControllerRoute(
                    name: "folderdefault",
                    pattern: "{sitefolder}/{controller}/{action}/{id?}",
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
            routes.MapControllerRoute(
                name: "errorhandler",
                pattern: "oops/error/{statusCode?}",
                defaults: new { controller = "Oops", action = "Error" }
                );
#if (Forum)
            routes.AddForumRoutes(new cloudscribe.Core.Web.Components.SiteFolderRouteConstraint());
#endif            

#if (ContactForm)
            routes.MapControllerRoute(
                name: "contact",
                pattern: "contact",
                defaults: new { controller = "Contact", action = "Index" }
                );
#endif

#if (SimpleContentConfig == "a" || SimpleContentConfig == "b")
            routes.MapControllerRoute(
                       name: "api-sitemap-culture",
                       pattern: "{culture}/api/sitemap"
                       , defaults: new { controller = "CultureSiteMap", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                       name: "api-rss-culture",
                       pattern: "{culture}/api/rss"
                       , defaults: new { controller = "CultureRss", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                       name: "api-metaweblog-culture",
                       pattern: "{culture}/api/metaweblog"
                       , defaults: new { controller = "CultureMetaweblog", action = "Index" }
                       , constraints: new { culture = new CultureSegmentRouteConstraint() }
                       );

            routes.MapControllerRoute(
                name: "sitemap-localized",
                pattern: "{culture}/sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                , constraints: new { culture = new CultureSegmentRouteConstraint() }
                );

            routes.MapControllerRoute(
                name: "sitemap",
                pattern: "sitemap"
                , defaults: new { controller = "Page", action = "SiteMap" }
                );
#endif
#if (SimpleContentConfig == "c")
            // you can easily add pages by uncommenting this and uncommenting the coresponding node in navigation.xml
            //routes.AddCustomPageRouteForSimpleContent("NONROOTPAGESEGMENT");
#endif
#if (SimpleContentConfig == "a" || SimpleContentConfig == "c")
            routes.MapControllerRoute(
                    name: "default-localized",
                    pattern: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );
                    
            routes.MapControllerRoute(
                name: "def",
                pattern: "{controller}/{action}"
                , defaults: new { action = "Index" }
                );
#endif
#if (SimpleContentConfig == "a")
            routes.AddCulturePageRouteForSimpleContent(new CultureSegmentRouteConstraint());
            routes.AddDefaultPageRouteForSimpleContent();
#endif
#if (SimpleContentConfig == "b")
            routes.AddCultureCustomPageRouteForSimpleContent(new CultureSegmentRouteConstraint(),"NONROOTPAGESEGMENT");
            routes.AddCustomPageRouteForSimpleContent("NONROOTPAGESEGMENT");
#endif

#if (SimpleContentConfig == "z" || SimpleContentConfig == "b" || SimpleContentConfig == "d")
            routes.MapControllerRoute(
                    name: "default-localized",
                    pattern: "{culture}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" },
                    constraints: new { culture = new CultureSegmentRouteConstraint() }
                    );

            routes.MapControllerRoute(
                name: "def",
                pattern: "{controller}/{action}"
                , defaults: new { controller = "Home", action = "Index" }
                );
#endif
#if (SimpleContentConfig == "c")
            //blog as default route
            routes.AddCultureBlogRoutesForSimpleContent(new CultureSegmentRouteConstraint(), "");
            routes.AddBlogRoutesForSimpleContent("");
#endif
            
            return routes;
        }

        public static IServiceCollection SetupMvc(
            this IServiceCollection services,
            bool sslIsAvailable
            )
        {
            services.Configure<MvcOptions>(options =>
            {
                // workaround for 
                //https://github.com/cloudscribe/cloudscribe.SimpleContent/issues/466
                options.EnableEndpointRouting = false;
                if (sslIsAvailable)
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                options.CacheProfiles.Add("SiteMapCacheProfile",
                     new CacheProfile
                     {
                         Duration = 30
                     });

#if (SimpleContentConfig == 'a' || SimpleContentConfig == 'b')

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
                    options.ViewLocationExpanders.Add(new cloudscribe.Core.Web.Components.SiteViewLocationExpander());
                })
                ;

            return services;
        }

    }
}
