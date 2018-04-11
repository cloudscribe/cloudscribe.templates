using Microsoft.Extensions.Configuration;
#if (KvpCustomRegistration)
using cloudscribe.UserProperties.Models;
using cloudscribe.UserProperties.Services;
#endif

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {
        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config
            )
        {
#if (!NoDb)
            var connectionString = config.GetConnectionString("EntityFrameworkConnection");

#if (SQLite)
            services.AddCloudscribeCoreEFStorageSQLite(connectionString);
#endif
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
#if (SQLite)
            services.AddCloudscribeKvpEFStorageSQLite(connectionString);
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
#if (SQLite)
            services.AddCloudscribeLoggingEFStorageSQLite(connectionString);
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
            
#endif
#if (SimpleContentConfig != "z")
#if (NoDb)
            services.AddNoDbStorageForSimpleContent();
#endif
#if (SQLite)
            services.AddCloudscribeSimpleContentEFStorageSQLite(connectionString);
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

            return services;
        }

        public static IServiceCollection SetupCloudscribeFeatures(
            this IServiceCollection services,
            IConfiguration config
            )
        {

#if (Logging)
            services.AddCloudscribeLogging();

#endif
#if (KvpCustomRegistration)
            services.Configure<ProfilePropertySetContainer>(config.GetSection("ProfilePropertySetContainer"));
            services.AddCloudscribeKvpUserProperties();
#endif

#if (ContactForm)
            services.AddCloudscribeSimpleContactForm(config);
#endif
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
#if (SimpleContentConfig != "z")
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();
#endif
            services.AddCloudscribeCoreMvc(config);
#if (SimpleContentConfig != "z")
            services.AddCloudscribeCoreIntegrationForSimpleContent(config);
            services.AddSimpleContentMvc(config);
            services.AddMetaWeblogForSimpleContent(config.GetSection("MetaWeblogApiOptions"));
            services.AddSimpleContentRssSyndiction();
#endif

            return services;
        }

    }
}
