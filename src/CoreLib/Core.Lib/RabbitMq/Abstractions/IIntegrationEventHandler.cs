using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;

namespace Core.Lib.RabbitMq.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }

}
