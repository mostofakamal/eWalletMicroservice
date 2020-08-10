using Core.Lib.RabbitMq.Configs;
using MassTransit;
using MassTransit.RabbitMqTransport.Topology.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using Core.Lib.RabbitMq.Abstractions;
using MassTransit.RabbitMqTransport;
using System.Linq;
using Core.Lib.IntegrationEvents;

namespace Core.Lib.RabbitMq
{
    public static class EventBusRabbitMq
    {
        public static QueueSettings QueueSettings;

        public static IServiceCollection RegisterQueueService(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("QueueSettings");
            QueueSettings = appSettingsSection.Get<QueueSettings>();
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(QueueSettings.HostName, QueueSettings.VirtualHost,
                     h =>
                     {
                         h.Username(QueueSettings.UserName);
                         h.Password(QueueSettings.Password);
                     });

                cfg.ExchangeType = ExchangeType.Direct;

                cfg.ReceiveEndpoint("customer-registered-kyc-endpoint", e =>
                {
                    var type = typeof(IConsumer<UserCreatedIntegrationEvent>);
                    var types = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => type.IsAssignableFrom(p));

                    foreach (var consumerType in types)
                    {
                        e.Consumer(consumerType, type => Activator.CreateInstance(consumerType));
                    }
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IHostedService, EventBusHostedService>();
            services.AddSingleton<IEventPublisher, EventPublisher>();

            return services;
        }
    }
}
