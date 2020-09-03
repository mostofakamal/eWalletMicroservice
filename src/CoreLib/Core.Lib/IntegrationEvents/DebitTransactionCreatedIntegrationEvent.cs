using System;

namespace Core.Lib.IntegrationEvents
{
    public class TransactionFailedIntegrationEvent : IntegrationEvent
    {
        public TransactionFailedIntegrationEvent(decimal amount, Guid senderUserGuid, Guid receiverUserGuid, int transactionType, Guid correlationId,string failedReason)
        {
            Amount = amount;
            SenderUserGuid = senderUserGuid;
            ReceiverUserGuid = receiverUserGuid;
            TransactionType = transactionType;
            CorrelationId = correlationId;
            FailedReason = failedReason;
        }
        public Guid CorrelationId { get; private set; }
        public Guid SenderUserGuid { get; private set; }

        public Guid ReceiverUserGuid { get; private set; }

        public decimal Amount { get; private set; }

        public int TransactionType { get; private set; }

        public string FailedReason { get; private set; }
    }
    public class RewardProcessStartedEvent : IntegrationEvent
    {
        public RewardProcessStartedEvent(decimal amount, Guid senderUserGuid, Guid receiverUserGuid, int transactionType, Guid correlationId)
        {
            Amount = amount;
            SenderUserGuid = senderUserGuid;
            ReceiverUserGuid = receiverUserGuid;
            TransactionType = transactionType;
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; private set; }
        public DateTime RequestStartedOn { get; private set; }

        public Guid SenderUserGuid { get; private set; }

        public Guid ReceiverUserGuid { get; private set; }

        public decimal Amount { get; private set; }

        public int TransactionType { get; private set; }

        public DateTime? RequestCancelledOn { get; private set; }
    }

    public class RewardProcessCancelledEvent : IntegrationEvent
    {
        public RewardProcessCancelledEvent(decimal amount, Guid senderUserGuid, Guid receiverUserGuid, Guid correlationId,string reason)
        {
            Amount = amount;
            Reason = reason;
            SenderUserGuid = senderUserGuid;
            ReceiverUserGuid = receiverUserGuid;
            CorrelationId = correlationId;

        }
        public Guid CorrelationId { get; private set; }
        public DateTime RequestStartedOn { get; private set; }

        public Guid SenderUserGuid { get; private set; }

        public Guid ReceiverUserGuid { get; private set; }

        public decimal Amount { get; private set; }

        public string Reason { get; private set; }

        public DateTime? RequestCancelledOn { get; private set; }

    }
    public class RewardProcessCompletedEvent: IntegrationEvent
    {
        public RewardProcessCompletedEvent(Guid correlationId,Guid rewardReceiverGuid)
        {
            CorrelationId = correlationId;
            RewardReceiverGuid = rewardReceiverGuid;
        }
        public Guid CorrelationId { get; private set; }

        public Guid RewardReceiverGuid { get; private set; }
    }

    public class DebitTransactionCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid TransactionGuid { get; private set; }

        public Guid SenderUserGuid { get; private set; }

        public Guid ReceiverUserGuid { get; private set; }

        public decimal Amount { get; private set; }

        public string TransactionType { get; private set; }

        public Guid CorrelationId { get; private set; }


        public DebitTransactionCreatedIntegrationEvent(decimal amount,Guid transactionGuid,Guid senderUserId,Guid receiverUserGuid,string transactionType,Guid correlationId)
        {
            Amount = amount;
            TransactionGuid = transactionGuid;
            SenderUserGuid = senderUserId;
            ReceiverUserGuid = receiverUserGuid;
            TransactionType = transactionType;
            CorrelationId = correlationId;
        }
    }
}