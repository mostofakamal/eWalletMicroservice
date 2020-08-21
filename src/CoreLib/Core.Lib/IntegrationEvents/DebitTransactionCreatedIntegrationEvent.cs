using System;

namespace Core.Lib.IntegrationEvents
{
    public class DebitTransactionCreatedIntegrationEvent : IntegrationEvent
    {
        public Guid TransactionGuid { get; private set; }

        public Guid SenderUserGuid { get; private set; }

        public Guid ReceiverUserGuid { get; private set; }

        public decimal Amount { get; private set; }

        public string TransactionType { get; private set; }
        public DebitTransactionCreatedIntegrationEvent(decimal amount,Guid transactionGuid,Guid senderUserId,Guid receiverUserGuid,string transactionType)
        {
            Amount = amount;
            TransactionGuid = transactionGuid;
            SenderUserGuid = senderUserId;
            ReceiverUserGuid = receiverUserGuid;
            TransactionType = transactionType;
        }
    }
}