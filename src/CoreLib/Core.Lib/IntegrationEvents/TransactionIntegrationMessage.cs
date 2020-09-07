using System;

namespace Core.Lib.IntegrationEvents
{
    public class TransactionIntegrationMessage : IntegrationMessage
    {
        public TransactionIntegrationMessage(decimal amount, Guid senderUserGuid, Guid receiverUserGuid, int transactionType, Guid correlationId, bool scheduleAndRetryIfFailed)
        {
            Amount = amount;
            SenderUserGuid = senderUserGuid;
            ReceiverUserGuid = receiverUserGuid;
            TransactionType = transactionType;
            CorrelationId = correlationId;
            ScheduleAndRetryIfFailed = scheduleAndRetryIfFailed;
        }
        public Guid SenderUserGuid { get; private set; }
        public Guid ReceiverUserGuid { get; private set; }
        public decimal Amount { get; private set; }
        public int TransactionType { get; private set; }

        public Guid CorrelationId { get; private set; }

        public bool ScheduleAndRetryIfFailed { get; private set; }
    }
}
