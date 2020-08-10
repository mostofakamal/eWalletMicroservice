using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Lib.RabbitMq.Abstractions
{
    public interface IEventPublisher
    {
        public Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : IntegrationEvent;

        public Task Publish(object message, Type messageType, CancellationToken cancellationToken = default);
    }
}
