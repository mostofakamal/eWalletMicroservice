﻿using System.Reflection;
using Core.Lib.Middlewares.Exceptions;
using Core.Lib.RabbitMq;
using Core.Lib.RabbitMq.Abstractions;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            //services.AddScoped<UserCreatedIntegrationEventConsumer>();
            //services.AddScoped<KycApprovedIntegrationEventConsumer>();
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTransactionService, UserTransactionService>();
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
            return services;
        }

        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<TransactionContext>().Database.Migrate();
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
                config.AddConsumer<UserCreatedIntegrationEventConsumer>();
                config.AddConsumer<KycApprovedIntegrationEventConsumer>();
                config.AddBus(EventBusRabbitMq.ConfigureBus);
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