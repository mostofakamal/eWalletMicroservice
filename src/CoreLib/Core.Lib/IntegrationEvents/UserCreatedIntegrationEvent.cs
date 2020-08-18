using System;

namespace Core.Lib.IntegrationEvents
{
    public class UserCreatedIntegrationEvent :  IUserCreatedIntegrationEvent
    {
        public Guid UserGuid { get; }

        public int CountryId { get; }

        public string PhoneNumber { get;}

        public UserCreatedIntegrationEvent(Guid userGuid,int countryId,string phoneNumber)
        {
            UserGuid = userGuid;
            CountryId = countryId;
            PhoneNumber = phoneNumber;
        }
    }
}