using System;
using System.Reflection;
using Core.Lib.IntegrationEvents;
using Core.Lib.Middlewares.Exceptions;
using Core.Lib.RabbitMq;
using Core.Lib.RabbitMq.Abstractions;
using Core.Services;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reward.API.Application.IntegrationEvents;
using Reward.Domain.AggregateModel;
using Reward.Domain.Services;
using Reward.Infrastructure.Repositories;
using RewardContext = Reward.Infrastructure.RewardContext;

namespace Reward.API.Infrastructure
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRewardService, UserRewardService>();
            services.AddScoped<IRewardService, RewardService>();
            return services;
        }
    }

    public static class CoreServiceRegistration
    {
        public static IServiceCollection RegisterDbAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<RewardContext>(options => options.UseSqlServer(
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
                scope.ServiceProvider.GetRequiredService<RewardContext>().Database.Migrate();
            }

            return app;
        }

        public static IApplicationBuilder ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<RewardExceptionMiddleware>();
            return app;
        }

        public static IServiceCollection ConfigQueue(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<UserCreatedIntegrationEventInRewardConsumer>();
                config.AddConsumer<RewardProcessCancelledEventConsumer>();
                config.AddConsumer<RewardProcessCompletedEventConsumer>();
                config.AddConsumer<KycApprovedIntegrationEventInRewardConsumer>();
                config.AddBus(provider => EventBusRabbitMq.ConfigureBus(provider));
            });

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IHostedService, EventBusHostedService>();
            services.AddSingleton<IEventPublisher, EventPublisher>();
            EndpointConvention.Map<TransactionIntegrationMessage>(new Uri($"queue:{nameof(TransactionIntegrationMessage)}"));
            return services;
        }
    }
}
