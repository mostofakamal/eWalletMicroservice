using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Lib.IntegrationEvents
{
    public interface IIntegrationEventPublisher<T> where T: IItegration
    {
        void PublishIntegrationEvent(T @event);
    }
}
