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
using MassTransit.Internals.Extensions;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.Lib.RabbitMq
{
    public static class EventBusRabbitMq
    {
        public static QueueSettings QueueSettings { get; private set; }

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
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IHostedService, EventBusHostedService>();
            services.AddSingleton<IEventPublisher, EventPublisher>();

            return services;
        }

        public static IBusControl ConfigureBus(IServiceProvider provider, Action<IRabbitMqBusFactoryConfigurator> config = null)
        {
            var queueSettings = provider.GetRequiredService<QueueSettings>();

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(queueSettings.HostName, queueSettings.VirtualHost, h =>
                {
                    h.Username(queueSettings.UserName);
                    h.Password(queueSettings.Password);
                });
                cfg.ConfigureEndpoints(provider);
                if (config != null) config.Invoke(cfg);
            });
            return busControl;
        }
    }
}
