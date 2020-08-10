using Core.Lib.RabbitMq.Abstractions;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;

namespace Core.Lib.RabbitMq
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _endpoint;

        public EventPublisher(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }
        public async Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : IntegrationEvent
        {
            await this._endpoint.Publish<T>(message);
        }

        public async Task Publish(object message, Type messageType, CancellationToken cancellationToken = default)
        {
            await this._endpoint.Publish(message, messageType);
        }
    }
}
