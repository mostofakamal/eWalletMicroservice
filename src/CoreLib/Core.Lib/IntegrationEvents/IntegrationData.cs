using System;
using Newtonsoft.Json;

namespace Core.Lib.IntegrationEvents
{
    public abstract class IntegrationData
    {
        protected IntegrationData()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        protected IntegrationData(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }

    public class IntegrationEvent: IntegrationData
    {
       
    }

    public class IntegrationMessage: IntegrationData
    {

    }
}
