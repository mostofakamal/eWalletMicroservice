using System;

namespace Core.Lib.IntegrationEvents
{
    public interface IUserCreatedIntegrationEvent 
    {
        Guid UserGuid { get; }
    }
}