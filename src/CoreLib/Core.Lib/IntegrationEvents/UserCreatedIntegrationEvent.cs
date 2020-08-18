using System;

namespace Core.Lib.IntegrationEvents
{
    public class UserCreatedIntegrationEvent :  IUserCreatedIntegrationEvent
    {
        public Guid UserGuid { get; }

        public int CountryId {get; }

        public UserCreatedIntegrationEvent(Guid userGuid, int countryId)
        {
            UserGuid = userGuid;
            CountryId = countryId;
        }
    }
}