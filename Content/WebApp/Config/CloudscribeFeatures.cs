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
            services.AddCloudscribeCorePostgreSqlStorage(connectionString);
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
            services.AddCloudscribeKvpPostgreSqlStorage(connectionString);
#endif
#endif
#if (Logging)
#if (NoDb)
            services.AddCloudscribeLoggingNoDbStorage(config);
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
            services.AddCloudscribeLoggingPostgreSqlStorage(connectionString);
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
            services.AddCloudscribeSimpleContentPostgreSqlStorage(connectionString);
#endif
#endif

#if (FormBuilder)
#if (NoDb)
            services.AddFormsStorageNoDb();
#endif
#if (SQLite)
            services.AddFormsStorageSQLite(connectionString);
#endif
#if (MSSQL)
            services.AddFormsStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddFormsStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddFormsStoragePostgreSql(connectionString);
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
            services.AddCloudscribeLogging(config);

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
            services.AddContentTemplatesForSimpleContent(config);
            
            services.AddMetaWeblogForSimpleContent(config.GetSection("MetaWeblogApiOptions"));
            services.AddSimpleContentRssSyndiction();
#endif

#if (FormBuilder)
            services.AddFormsCloudscribeCoreIntegration(config);
            services.AddFormsServices(config);
#if (SimpleContentConfig != "z")
            services.AddFormSurveyContentTemplatesForSimpleContent(config);
#endif
            // these are examples to show you how to implement custom form submission handlers.
            // see /Services/SampleFormSubmissionHandlers.cs
            services.AddScoped<cloudscribe.Forms.Models.IHandleFormSubmission, WebApp.Services.FakeFormSubmissionHandler1>();
            services.AddScoped<cloudscribe.Forms.Models.IHandleFormSubmission, WebApp.Services.FakeFormSubmissionHandler2>();
#endif



            return services;
        }

    }
}
