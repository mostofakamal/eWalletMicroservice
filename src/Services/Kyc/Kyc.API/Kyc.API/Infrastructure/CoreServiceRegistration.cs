using Core.Lib.RabbitMq;
using Core.Lib.RabbitMq.Abstractions;
using Core.Lib.Repository;
using Kyc.API.Application.IntegrationEvents;
using Kyc.Domain.AggregateModel;
using Kyc.Insfrastructure;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kyc.API.Infrastructure
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection RegisterDbAccess(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<KycContext>(options => options.UseSqlServer(
               config.GetConnectionString("DefaultConnection"),
               b =>
               {
                   b.MigrationsAssembly("Kyc.API");
                   //b.EnableRetryOnFailure(5);
               }));
            services.AddScoped<IKycRepository, KycRepository>();
            return services;
        }

        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<KycContext>().Database.Migrate();
            }

            return app;
        }

        public static IServiceCollection ConfigQueue(this IServiceCollection services)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<UserCreatedIntegratedEventConsumer>();
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
