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
            IConfiguration config,
            bool useHangfire
            )
        {
#if (!NoDb)
            var connectionString = config.GetConnectionString("EntityFrameworkConnection");

#if (SQLite)
            services.AddCloudscribeCoreEFStorageSQLite(connectionString);
#endif

#if (IncludeHangfire)

#if (MSSQL)
            if(useHangfire)
            {
                services.AddHangfire(hfConfig => hfConfig.UseSqlServerStorage(msSqlConnectionString));
            }
#endif
#if (MySql)
            if (useHangfire)
            {
                services.AddHangfire(hfConfig => { });

                GlobalConfiguration.Configuration.UseStorage(
                    new MySqlStorage(
                        connectionString + "Allow User Variables=True",
                        new MySqlStorageOptions
                        {
                            //TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                            //QueuePollInterval = TimeSpan.FromSeconds(15),
                            //JobExpirationCheckInterval = TimeSpan.FromHours(1),
                            //CountersAggregateInterval = TimeSpan.FromMinutes(5),
                            //PrepareSchemaIfNecessary = true,
                            //DashboardJobListLimit = 50000,
                            //TransactionTimeout = TimeSpan.FromMinutes(1),
                        }));
            }
#endif
#if (pgsql)
            if (useHangfire)
            {
                services.AddHangfire(hfConfig => hfConfig.UsePostgreSqlStorage(connectionString));
            }
#endif

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
#if (MSSQL)
            services.AddMembershipSubscriptionStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddMembershipSubscriptionStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddMembershipSubscriptionPostgreSqlStorage(connectionString);
#endif        
#endif

#if (Newsletter)
#if (MSSQL)
            services.AddEmailListStorageMSSQL(connectionString);
#endif
#if (MySql)
            services.AddEmailListStorageMySql(connectionString);
#endif
#if (pgsql)
            services.AddEmailListPostgreSqlStorage(connectionString);
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

#if (IncludeStripeIntegration)
            services.AddMembershipSStripeIntegration(config);
            services.AddCloudscribeCoreStripeIntegration();
            services.AddStripeIntegrationMvc(config);

#endif

#if (Paywall)
            services.AddScoped<IRoleRemovalTask, HangfireRoleRemovalTask>();
            services.AddScoped<ISendRemindersTask, HangfireSendRemindersTask>();
            services.AddMembershipSubscriptionMvcComponents(config);
#endif

#if (IncludeEmailQueue)
            services.AddScoped<IEmailQueueProcessor, HangFireEmailQueueProcessor>();
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

            return services;
        }

    }
}
