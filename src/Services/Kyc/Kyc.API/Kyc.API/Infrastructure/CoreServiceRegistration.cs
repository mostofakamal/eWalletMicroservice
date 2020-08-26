using Core.Lib.IntegrationEvents;
using Core.Lib.RabbitMq;
using Core.Lib.RabbitMq.Abstractions;
using IntegrationDataLog;
using Kyc.API.Application.IntegrationEvents;
using Kyc.Domain.AggregateModel;
using Kyc.Insfrastructure;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace Kyc.API.Infrastructure
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection RegisterDbAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<UserContext>(options => options.UseSqlServer(
               config.GetConnectionString("DefaultConnection"),
               b =>
               {
                   b.MigrationsAssembly("Kyc.API");
                   //b.EnableRetryOnFailure(5);
               }));
            //services.AddDbContext<IntegrationDataLogContext>(options =>
            //{
            //    options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
            //        sqlServerOptionsAction: sqlOptions =>
            //        {
            //            sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            //            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            //        });
            //});
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddDbContext<IntegrationDataLogContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });
            return services;
        }

        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<UserContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<IntegrationDataLogContext>().Database.Migrate();
            }
            return app;
        }

        public static IServiceCollection ConfigQueue(this IServiceCollection services)
        {
            services.AddTransient<UserCreatedIntegratedKEventInKycConsumer>();
            services.AddTransient<TransactionIntegrationMessageConsumer>();

            EndpointConvention.Map<ITransactionIntegrationMessage>(new Uri($"queue:{nameof(ITransactionIntegrationMessage)}"));

            services.AddMassTransit(config =>
            {
                config.AddBus(provider =>
                {
                    var busControl = EventBusRabbitMq.ConfigureBus(provider);
                    return busControl;
                });
                config.AddConsumer<UserCreatedIntegratedKEventInKycConsumer>();
            });

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IHostedService, EventBusHostedService>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            return services;
        }
    }
}
