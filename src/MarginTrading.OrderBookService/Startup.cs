// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.ApiLibrary.Middleware;
using Lykke.Common.ApiLibrary.Swagger;
using Lykke.Logs;
using Lykke.Logs.MsSql;
using Lykke.Logs.MsSql.Repositories;
using Lykke.Logs.Serilog;
using Lykke.MarginTrading.OrderBookService.Contracts.Api;
using Lykke.SettingsReader;
using Lykke.Snow.Common.Startup;
using Lykke.Snow.Common.Startup.ApiKey;
using Lykke.Snow.Common.Startup.Hosting;
using Lykke.Snow.Common.Startup.Log;
using MarginTrading.OrderBookService.Core.Modules;
using MarginTrading.OrderBookService.Infrastructure;
using MarginTrading.OrderBookService.Modules;
using MarginTrading.OrderBookService.Services;
using MarginTrading.OrderBookService.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MarginTrading.OrderBookService
{
    [UsedImplicitly]
    public class Startup
    {
        private IReloadingManager<AppSettings> _mtSettingsManager;
        public static string ServiceName { get; } = PlatformServices.Default.Application.ApplicationName;
        private IHostEnvironment Environment { get; }
        private ILifetimeScope ApplicationContainer { get; set; }
        private IConfigurationRoot Configuration { get; }
        [CanBeNull] private ILog Log { get; set; }

        public Startup(IHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddSerilogJson(env)
                .AddEnvironmentVariables()
                .Build();
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });

                _mtSettingsManager = Configuration.LoadSettings<AppSettings>();

                services.AddApiKeyAuth(_mtSettingsManager.CurrentValue.OrderBookServiceClient);

                services.AddSwaggerGen(options =>
                {
                    options.DefaultLykkeConfiguration("v1", ServiceName + " API");
                    options.OperationFilter<CustomOperationIdOperationFilter>();
                    if (!string.IsNullOrWhiteSpace(_mtSettingsManager.CurrentValue.OrderBookServiceClient?.ApiKey))
                    {
                        options.AddApiKeyAwareness();
                    }
                });

                services.AddApplicationInsightsTelemetry();

                Log = CreateLog(Configuration, services, _mtSettingsManager);

                services.AddSingleton<ILoggerFactory>(x => new WebHostLoggerFactory(Log));
            }
            catch (Exception ex)
            {
                Log?.WriteFatalErrorAsync(nameof(Startup), nameof(ConfigureServices), "", ex).Wait();
                throw;
            }
        }
        
        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterModule(new OrderBookServiceModule(_mtSettingsManager, Log));
            builder.RegisterModule(new RedisModule(_mtSettingsManager.CurrentValue.OrderBookService.Db.RedisSettings
                .Configuration));
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            try
            {
                ApplicationContainer = app.ApplicationServices.GetAutofacRoot();
                
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseHsts();
                }

#if DEBUG
                app.UseLykkeMiddleware(ServiceName, ex => ex.ToString(), false, false);
#else
                app.UseLykkeMiddleware(ServiceName, ex => new ErrorResponse {ErrorMessage = "Technical problem", Details = ex.Message}, false, false);
#endif

                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
                app.UseSwagger();
                app.UseSwaggerUI(a => a.SwaggerEndpoint("/swagger/v1/swagger.json", "Main Swagger"));

                appLifetime.ApplicationStarted.Register(() => StartApplication().Wait());
                appLifetime.ApplicationStopping.Register(() => StopApplication().Wait());
                appLifetime.ApplicationStopped.Register(() => CleanUp().Wait());
            }
            catch (Exception ex)
            {
                Log?.WriteFatalErrorAsync(nameof(Startup), nameof(ConfigureServices), "", ex).Wait();
                throw;
            }
        }

        private Task StartApplication()
        {
            try
            {
                Program.AppHost.WriteLogs(Environment, LogLocator.Log);

                Log?.WriteMonitorAsync("", "", "Started").Wait();
            }
            catch (Exception ex)
            {
                Log?.WriteFatalErrorAsync(nameof(Startup), nameof(StartApplication), "", ex).Wait();
                throw;
            }

            return Task.CompletedTask;
        }

        private async Task StopApplication()
        {
            try
            {
                // NOTE: Service still can receive and process requests here, so take care about it if you add logic here.
            }
            catch (Exception ex)
            {
                if (Log != null)
                {
                    await Log.WriteFatalErrorAsync(nameof(Startup), nameof(StopApplication), "", ex);
                }

                throw;
            }
        }

        private async Task CleanUp()
        {
            try
            {
                // NOTE: Service can't receive and process requests here, so you can destroy all resources

                if (Log != null)
                {
                    await Log.WriteMonitorAsync("", "", "Terminating");
                }

                ApplicationContainer.Dispose();
            }
            catch (Exception ex)
            {
                if (Log != null)
                {
                    await Log.WriteFatalErrorAsync(nameof(Startup), nameof(CleanUp), "", ex);
                    (Log as IDisposable)?.Dispose();
                }

                throw;
            }
        }

        private static ILog CreateLog(IConfiguration configuration, IServiceCollection services,
            IReloadingManager<AppSettings> settings)
        {
            var logName = $"{nameof(OrderBookService)}Log";

            var consoleLogger = new LogToConsole();
            var aggregateLogger = new AggregateLogger();

            aggregateLogger.AddLog(consoleLogger);

            if (settings.CurrentValue.OrderBookService.UseSerilog)
            {
                aggregateLogger.AddLog(new SerilogLogger(typeof(Startup).Assembly, configuration));
            }
            else if (settings.CurrentValue.OrderBookService.Db.StorageMode == StorageMode.SqlServer)
            {
                aggregateLogger.AddLog(new LogToSql(new SqlLogRepository(logName,
                    settings.CurrentValue.OrderBookService.Db.LogsConnString)));
            }
            else if (settings.CurrentValue.OrderBookService.Db.StorageMode == StorageMode.Azure)
            {
                aggregateLogger.AddLog(services.UseLogToAzureStorage(settings.Nested(s => s.OrderBookService.Db.LogsConnString),
                    null, logName, consoleLogger));
            }

            LogLocator.Log = aggregateLogger;

            return aggregateLogger;
        }
    }
}