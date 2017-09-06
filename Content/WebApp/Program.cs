using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            var env = host.Services.GetRequiredService<IHostingEnvironment>();
            #if (Logging)
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            ConfigureLogging(env, loggerFactory, host.Services);
            #endif
            using (var scope = host.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider; 
                try
                {
                    EnsureDataStorageIsReady(scopedServices);

                }
                catch (Exception ex)
                {
                    var logger = scopedServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private static void EnsureDataStorageIsReady(IServiceProvider scopedServices)
        {
            #if (NoDb)
            CoreNoDbStartup.InitializeDataAsync(scopedServices).Wait();
            #else
            CoreEFStartup.InitializeDatabaseAsync(scopedServices).Wait();
            #endif
            
            #if (SimpleContent)
            #if (!NoDb)
            SimpleContentEFStartup.InitializeDatabaseAsync(scopedServices).Wait();
            #endif
            #endif

            #if (IdentityServer)
            #if (NoDb)
            CloudscribeIdentityServerIntegrationNoDbStorage.InitializeDatabaseAsync(scopedServices).Wait();
            #else
            CloudscribeIdentityServerIntegrationEFCoreStorage.InitializeDatabaseAsync(scopedServices).Wait();
            #endif
            #endif

            #if (Logging)
            #if (!NoDb)
            LoggingEFStartup.InitializeDatabaseAsync(scopedServices).Wait();
            #endif
            #endif
        }

#if (Logging)
        private static void ConfigureLogging(
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            )
        {
            LogLevel minimumLevel;
            if (env.IsProduction())
            {
                minimumLevel = LogLevel.Warning;
            }
            else
            {
                minimumLevel = LogLevel.Information;
            }

            // a customizable filter for logging
            // add exclusions to remove noise in the logs
            var excludedLoggers = new List<string>
            {
                "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
                "Microsoft.AspNetCore.Hosting.Internal.WebHost",
            };

            Func<string, LogLevel, bool> logFilter = (string loggerName, LogLevel logLevel) =>
            {
                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (excludedLoggers.Contains(loggerName))
                {
                    return false;
                }

                return true;
            };

            loggerFactory.AddDbLogger(serviceProvider, logFilter);
        }
#endif

    }


}
