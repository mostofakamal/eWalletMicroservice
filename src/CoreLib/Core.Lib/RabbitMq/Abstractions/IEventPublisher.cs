using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;

namespace Core.Lib.RabbitMq.Abstractions
{
    public interface IEventPublisher
    {
         Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class;
         
         Task Publish(object message, Type messageType, CancellationToken cancellationToken = default);
    }
}
