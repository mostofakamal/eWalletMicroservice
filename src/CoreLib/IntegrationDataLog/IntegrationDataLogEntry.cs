using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Core.Lib.IntegrationEvents;
using Newtonsoft.Json;

namespace IntegrationDataLog
{
    public class IntegrationDataLogEntry
    {
        private IntegrationDataLogEntry() { }
        public IntegrationDataLogEntry(IntegrationData data, Guid transactionId)
        {
            IntegrationDataId = data.Id;
            CreationTime = data.CreationDate;
            DataTypeName = data.GetType().FullName;
            Content = JsonConvert.SerializeObject(data);
            State = IntegrationDataStateEnum.NotPublished;
            IntegrationDataType = data is IntegrationEvent ? IntegrationDataType.Event: IntegrationDataType.Message;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }
        public Guid IntegrationDataId { get; private set; }
        public string DataTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => DataTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationData IntegrationData { get; private set; }
        public IntegrationDataStateEnum State { get; set; }

        public IntegrationDataType IntegrationDataType { get; set; }
        
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string TransactionId { get; private set; }

        public IntegrationDataLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationData = JsonConvert.DeserializeObject(Content, type) as IntegrationData;
            return this;
        }
    }
}
