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
        private readonly ISendEndpointProvider sendEndpoint;

        public EventPublisher(IPublishEndpoint endpoint, ISendEndpointProvider sendEndpoint)
        {
            _endpoint = endpoint;
            this.sendEndpoint = sendEndpoint;
        }
        public async Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            await this._endpoint.Publish<T>(message);
        }

        public async Task Publish(object message, Type messageType, CancellationToken cancellationToken = default)
        {
            await this._endpoint.Publish(message, messageType);
        }

        public async Task Send(object message, Type messageType, CancellationToken cancellationToken = default)
        {
            await this.sendEndpoint.Send(message, messageType);
        }
    }
}
