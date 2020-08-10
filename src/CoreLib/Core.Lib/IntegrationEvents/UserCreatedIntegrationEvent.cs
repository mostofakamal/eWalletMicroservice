using System;

namespace Core.Lib.IntegrationEvents
{
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserGuid { get; }


        public UserCreatedIntegrationEvent(Guid userGuid)
        {
            UserGuid = userGuid;
        }
    }
}