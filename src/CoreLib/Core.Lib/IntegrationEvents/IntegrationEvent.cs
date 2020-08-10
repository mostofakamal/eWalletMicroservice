﻿using System;
using Newtonsoft.Json;

namespace Core.Lib.IntegrationEvents
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }

    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid UserGuid { get; }


        public UserCreatedIntegrationEvent(Guid userGuid)
        {
            UserGuid = userGuid;
        }
    }
}
