#if (QueryTool && !NoDb)
using cloudscribe.QueryTool.EFCore.Common;
#endif
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var hostBuilder = CreateHostBuilder(args);
                var host = hostBuilder.Build();
                IConfiguration config;
                
                using (var scope = host.Services.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    config = scopedServices.GetRequiredService<IConfiguration>();
                    try
                    {
#if (AllStorage)
                        await EnsureDataStorageIsReady(config, scopedServices);
#else
                        await EnsureDataStorageIsReady(scopedServices);
#endif
                    }
                    catch (Exception ex)
                    {
                        var logger = scopedServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating the database.");
                    }
                }

#if (Logging)
                var env = host.Services.GetRequiredService<IWebHostEnvironment>();
                var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
                ConfigureLogging(env, loggerFactory, host.Services, config);
#endif

                await host.RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Host terminated unexpectedly.");
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

#if (!AllStorage)
        private static async Task EnsureDataStorageIsReady(IServiceProvider scopedServices)
        {
#if (Logging)
#if (!NoDb)
            var deletePostsOlderThanDays = 90;
            await LoggingEFStartup.InitializeDatabaseAsync(scopedServices, deletePostsOlderThanDays);
#endif
#endif
#if (NoDb)
            await CoreNoDbStartup.InitializeDataAsync(scopedServices);
#else
            await CoreEFStartup.InitializeDatabaseAsync(scopedServices);
#endif
#if (SimpleContentConfig != "z")
#if (!NoDb)
            await SimpleContentEFStartup.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (KvpCustomRegistration || Newsletter)
#if (!NoDb)
            await KvpEFCoreStartup.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (IdentityServer)
#if (NoDb)
            await CloudscribeIdentityServerIntegrationNoDbStorage.InitializeDatabaseAsync(scopedServices);
#else
            await CloudscribeIdentityServerIntegrationEFCoreStorage.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (FormBuilder)
#if (!NoDb)
            await FormsDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (Paywall)
#if (!NoDb)
            await MembershipDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (IncludeEmailQueue)
#if (!NoDb)
            await EmailQueueDatabase.InitializeDatabaseAsync(scopedServices);
            await EmailTemplateDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (Newsletter)
#if (!NoDb)
            await EmailListDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (IncludeStripeIntegration)
#if (!NoDb)
            await StripeDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (DynamicPolicy)
#if (!NoDb)
            await DynamicPolicyEFCore.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (CommentSystem)
#if (!NoDb)
            await CommentsDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (Forum)
#if (!NoDb)
            await ForumDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#endif
#if (QueryTool)
#if (!NoDb)
            await QueryToolStartup.InitializeDatabaseAsync(scopedServices);
#endif
#endif
        }
#endif
#if (AllStorage)
        private static async Task EnsureDataStorageIsReady(IConfiguration config, IServiceProvider scopedServices)
        {
            var storage = config["DevOptions:DbPlatform"].ToLowerInvariant();

            switch (storage)
            {
                case "efcore":
                    await CoreEFStartup.InitializeDatabaseAsync(scopedServices);
#if (Logging)
                    var deletePostsOlderThanDays = 90;
                    await LoggingEFStartup.InitializeDatabaseAsync(scopedServices, deletePostsOlderThanDays);
#endif
#if (SimpleContentConfig != "z")
                    await SimpleContentEFStartup.InitializeDatabaseAsync(scopedServices);
#endif
#if (KvpCustomRegistration || Newsletter)
                    await KvpEFCoreStartup.InitializeDatabaseAsync(scopedServices);
#endif
#if (IdentityServer)
                    await CloudscribeIdentityServerIntegrationEFCoreStorage.InitializeDatabaseAsync(scopedServices);
#endif
#if (FormBuilder)
                    await FormsDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#if (Paywall)
                    await MembershipDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#if (IncludeEmailQueue)
                    await EmailQueueDatabase.InitializeDatabaseAsync(scopedServices);
                    await EmailTemplateDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#if (Newsletter)
                    await EmailListDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#if (IncludeStripeIntegration)
                    await StripeDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#if (DynamicPolicy)
                    await DynamicPolicyEFCore.InitializeDatabaseAsync(scopedServices);
#endif
#if (CommentSystem)
                    await CommentsDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#if (Forum)
                    await ForumDatabase.InitializeDatabaseAsync(scopedServices);
#endif
#if (QueryTool)
                    await QueryToolStartup.InitializeDatabaseAsync(scopedServices);
#endif
                    break;
                
                case "nodb":
                default:
                    await CoreNoDbStartup.InitializeDataAsync(scopedServices);
                    break;
            }
        }
#endif
#if (Logging)

        private static void ConfigureLogging(
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider,
            IConfiguration config
            )
        {
            var dbLoggerConfig = config.GetSection("DbLoggerConfig").Get<cloudscribe.Logging.Models.DbLoggerConfig>();
            LogLevel minimumLevel;
            string levelConfig;
            if (env.IsProduction())
            {
                levelConfig = dbLoggerConfig.ProductionLogLevel;
            }
            else
            {
                levelConfig = dbLoggerConfig.DevLogLevel;
            }
            switch (levelConfig)
            {
                case "Debug":
                    minimumLevel = LogLevel.Debug;
                    break;

                case "Information":
                    minimumLevel = LogLevel.Information;
                    break;

                case "Trace":
                    minimumLevel = LogLevel.Trace;
                    break;

                default:
                    minimumLevel = LogLevel.Warning;
                    break;
            }
            
            // a customizable filter for logging
            // add exclusions in appsettings.json to remove noise in the logs
            bool logFilter(string loggerName, LogLevel logLevel)
            {
                if (dbLoggerConfig.ExcludedNamesSpaces.Any(f => loggerName.StartsWith(f)))
                {
                    return false;
                }

                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (dbLoggerConfig.BelowWarningExcludedNamesSpaces.Any(f => loggerName.StartsWith(f)) && logLevel < LogLevel.Warning)
                {
                    return false;
                }
                return true;
            }

            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }
#endif
    }
}
