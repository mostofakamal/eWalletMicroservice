using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.IntegrationEvents
{
    public interface IIntegrationEventPublisher<T>
    {
        void PublishIntegrationEvent(T @event);
    }
}
