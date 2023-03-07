using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
#if (KvpCustomRegistration || Newsletter)
using cloudscribe.UserProperties.Models;
using cloudscribe.UserProperties.Services;
#endif

#if (QueryTool && !NoDb)
using cloudscribe.QueryTool.Services;
#if (MSSQL)
using cloudscribe.QueryTool.EFCore.MSSQL;
#endif
#if (MySql)
using cloudscribe.QueryTool.EFCore.MySql;
#endif
#if (pgsql)
using cloudscribe.QueryTool.EFCore.PostgreSql;
#endif
#if (SQLite)
using cloudscribe.QueryTool.EFCore.SQLite;
#endif
#endif

using Microsoft.Extensions.Configuration;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {
        
        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment env
            )
        {
#if (!NoDb && !SQLite)
            var connectionString = config.GetConnectionString("EntityFrameworkConnection");
#endif


#if (SQLite)
            var dbName = config.GetConnectionString("SQLiteDbName");
            var dbPath = Path.Combine(env.ContentRootPath, dbName);
            var connectionString = $"Data Source={dbPath}";
            services.AddCloudscribeCoreEFStorageSQLite(connectionString);
#endif
#if (!NoDb && !SQLite)

#if (MSSQL)
            services.AddCloudscribeCoreEFStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddCloudscribeCoreEFStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddCloudscribeCorePostgreSqlStorage(connectionString);
#endif

#endif

#if (NoDb)
            services.AddCloudscribeCoreNoDbStorage();
#endif
#if (KvpCustomRegistration || Newsletter)
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

#if (IncludeStripeIntegration)
#if (NoDb)
            services.AddStripeIntegrationStorageNoDb();
#endif
#if (SQLite)
             services.AddStripeIntegrationStorageSQLite(connectionString);
#endif
#if (MSSQL)
            services.AddStripeIntegrationStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddStripeIntegrationStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddStripeIntegrationPostgreSqlStorage(connectionString);
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

#if (DynamicPolicy)
#if (NoDb)
            services.AddNoDbStorageForDynamicPolicies(config);
#endif
#if (SQLite)
            services.AddDynamicPolicyEFStorageSQLite(connectionString);
#endif
#if (MSSQL)
            services.AddDynamicPolicyEFStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddDynamicPolicyEFStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddDynamicPolicyPostgreSqlStorage(connectionString);
#endif
#endif

#if (Paywall)
#if (NoDb)
            services.AddMembershipSubscriptionStorageNoDb();
#endif
#if (MSSQL)
            services.AddMembershipSubscriptionStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddMembershipSubscriptionStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddMembershipSubscriptionPostgreSqlStorage(connectionString);
#endif
#if (SQLite)
            services.AddMembershipSubscriptionStorageSQLite(connectionString);
#endif
#endif

#if (IncludeEmailQueue)
#if (NoDb)
            services.AddEmailTemplateStorageNoDb();
            services.AddEmailQueueStorageNoDb();
#endif
#if (MSSQL)
            services.AddEmailTemplateStorageMSSQL(connectionString);
            services.AddEmailQueueStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddEmailTemplateStorageMySql(connectionString);
            services.AddEmailQueueStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddEmailTemplatePostgreSqlStorage(connectionString);
            services.AddEmailQueuePostgreSqlStorage(connectionString);
#endif
#if (SQLite)
             services.AddEmailTemplateStorageSQLite(connectionString);
             services.AddEmailQueueStorageSQLite(connectionString);
#endif
#endif

#if (Newsletter)
#if (NoDb)
            services.AddEmailListStorageNoDb();
#endif
#if (MSSQL)
            services.AddEmailListStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddEmailListStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddEmailListPostgreSqlStorage(connectionString);
#endif
#if (SQLite)
            services.AddEmailListStorageSQLite(connectionString);
#endif
#endif

#if (CommentSystem)
#if (NoDb)
            services.AddTalkAboutStorageNoDb();
#endif
#if (SQLite)
            services.AddCommentStorageSQLite(connectionString);
#endif
#if (MSSQL)
            services.AddCommentStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddCommentStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddCommentStoragePostgreSql(connectionString);
#endif
#endif

#if (Forum)
#if (NoDb)
            services.AddTalkAboutForumStorageNoDb();
#endif
#if (SQLite)
            services.AddForumStorageSQLite(connectionString);
#endif
#if (MSSQL)
            services.AddForumStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddForumStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddForumStoragePostgreSql(connectionString);
#endif
#endif


#if (QueryTool && !NoDb)
#if (SQLite)
            services.AddQueryToolEFStorageSQLite(connectionString:connectionString,maxConnectionRetryCount:0,maxConnectionRetryDelaySeconds:30,commandTimeout:30);
#endif
#if (MSSQL)
            services.AddQueryToolEFStorageMSSQL(connectionString:connectionString,maxConnectionRetryCount:0,maxConnectionRetryDelaySeconds:30,transientSqlErrorNumbersToAdd:null);
#endif
#if (MySql)
            services.AddQueryToolEFStorageMySql(connectionString:connectionString,maxConnectionRetryCount:0,maxConnectionRetryDelaySeconds:30,transientSqlErrorNumbersToAdd:null);
#endif
#if (pgsql)
            services.AddQueryToolEFStoragePostgreSql(connectionString:connectionString,maxConnectionRetryCount:0,maxConnectionRetryDelaySeconds:30,transientErrorCodesToAdd:null);
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
#if (KvpCustomRegistration || Newsletter)
            services.Configure<ProfilePropertySetContainer>(config.GetSection("ProfilePropertySetContainer"));
#if (Newsletter) 
            services.AddEmailListKvpIntegration(config);
#endif
            services.AddCloudscribeKvpUserProperties();
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
#if (ContactForm)
            services.AddCloudscribeSimpleContactFormCoreIntegration(config);
            services.AddCloudscribeSimpleContactForm(config);
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

#if (IncludeStripeIntegration)
            services.AddMembershipStripeIntegration(config);
            services.AddCloudscribeCoreStripeIntegration();
            services.AddStripeIntegrationMvc(config);

#endif

#if (Paywall)
            services.AddMembershipSubscriptionMvcComponents(config);
            services.AddMembershipBackgroundTasks(config);
#endif

#if (IncludeEmailQueue)
            services.AddEmailQueueBackgroundTask(config);
            services.AddEmailQueueWithCloudscribeIntegration(config);
            services.AddEmailRazorTemplating(config);
#endif

#if (Newsletter)
            services.AddEmailListWithCloudscribeIntegration(config);
#endif

#if (DynamicPolicy)
            services.AddCloudscribeDynamicPolicyIntegration(config);
            services.AddDynamicAuthorizationMvc(config);
#endif

#if (CommentSystem || Forum)
            services.AddTalkAboutCloudscribeIntegration(config);    
#endif

#if (Forum)
  
            services.AddTalkAboutForumServices(config)
                .AddTalkAboutForumNotificationServices(config);
#endif

#if (CommentSystem)
            services.AddTalkAboutCommentsCloudscribeIntegration(config);
            services.AddTalkAboutServices(config)
                .AddTalkAboutNotificationServices(config);
#endif


#if (QueryTool && !NoDb)
            //The QueryTool can only work with Entity Framework databases and not with NoDb
            services.AddScoped<IQueryTool,QueryTool>();
#endif

            return services;
        }

    }
}
