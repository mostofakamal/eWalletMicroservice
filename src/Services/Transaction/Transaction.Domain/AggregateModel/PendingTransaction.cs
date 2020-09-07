using System;
using Transaction.Domain.SeedWork;

namespace Transaction.Domain.AggregateModel
{
    public class PendingTransaction : Entity
    {
        public PendingTransaction()
        {
        }

        public PendingTransaction(decimal amount, int senderUserId, int receiverGuid, TransactionType type, Guid? correlationId)
        {
            Amount = amount;
            _senderUserId = senderUserId;
            _receiverUserId = receiverGuid;
            _transactionTypeId = type.Id;
            CorrelationId = correlationId;
            ScheduledOn = DateTime.UtcNow;
        }

        public decimal Amount { get; private set; }

        private int _senderUserId;

        public User SenderUser { get; private set; }

        private int _receiverUserId;

        public User ReceiverUser { get; private set; }

        public TransactionType TransactionType { get; private set; }

        private int _transactionTypeId;

        public Guid? CorrelationId { get; private set; }

        public DateTime ScheduledOn { get; private set; }

        public DateTime?  HandledOn { get; private set; }

        public void MarkAsHandled()
        {
            HandledOn = DateTime.UtcNow;
        }
    }
}