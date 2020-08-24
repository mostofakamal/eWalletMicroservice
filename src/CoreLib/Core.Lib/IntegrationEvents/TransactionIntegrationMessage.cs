using System;

namespace Core.Lib.IntegrationEvents
{
    public class TransactionIntegrationMessage : ITransactionIntegrationMessage
    {
        public Guid SenderUserGuid { get; set; }
        public Guid ReceiverUserGuid { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
    }
}
