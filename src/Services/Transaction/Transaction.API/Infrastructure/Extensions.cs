using System;
using System.Data.Common;
using System.Reflection;
using Core.Lib.IntegrationEvents;
using Core.Lib.Middlewares.Exceptions;
using Core.Lib.RabbitMq;
using Core.Lib.RabbitMq.Abstractions;
using IntegrationDataLog;
using IntegrationDataLog.Services;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Transaction.API.Application.Behaviours;
using Transaction.API.Application.IntegrationEvents;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;
using Transaction.Infrastructure.Repositories;
using TransactionContext = Transaction.Infrastructure.TransactionContext;

namespace Transaction.API.Infrastructure
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTransactionService, UserTransactionService>();
            services.AddTransient<Func<DbConnection, ILogger<IIntegrationDataLogService>, IIntegrationDataLogService>>(
                sp => (c, l)=> new IntegrationDataLogService(c,l));

            services.AddTransient<ITransactionIntegrationDataService, TransactionIntegrationDataService>();

            return services;
        }
    }

    public static class CoreServiceRegistration
    {
        public static IServiceCollection RegisterDbAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<TransactionContext>(options => options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                b =>
                {
                    b.MigrationsAssembly(typeof(Startup).Assembly.FullName);
                    //b.EnableRetryOnFailure(5);
                }));
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
                scope.ServiceProvider.GetRequiredService<TransactionContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<IntegrationDataLogContext>().Database.Migrate();
            }

            return app;
        }

        public static IApplicationBuilder ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<TransactionExceptionMiddleware>();
            return app;
        }

        public static IServiceCollection ConfigQueue(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<UserCreatedIntegrationEventInTransactionConsumer>();
                config.AddConsumer<KycApprovedIntegrationEventInTransactionConsumer>();

                config.AddBus(provider => {
                    var busControl = EventBusRabbitMq.ConfigureBus(provider, cfg => {
                        cfg.ReceiveEndpoint(nameof(TransactionIntegrationMessage), ep =>
                        {
                            ep.Consumer<TransactionIntegrationEventConsumer>();
                        });
                    });
                    return busControl;
                });
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
