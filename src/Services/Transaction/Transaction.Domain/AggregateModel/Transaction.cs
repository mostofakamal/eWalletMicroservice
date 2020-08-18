using System;
using Transaction.Domain.SeedWork;

namespace Transaction.Domain.AggregateModel
{
    public class Transaction : Entity
    {
        public Transaction()
        {
            
        }
        public Transaction(decimal amount,Guid counterPartyUserGuid,TransactionType type)
        {
            Amount = amount;
            CounterPartyUserGuid = counterPartyUserGuid;
            _transactionTypeId = type.Id;
            TransactionGuid = Guid.NewGuid();
            _transactionStatusId = TransactionStatus.Ok.Id;
        }
        public Guid TransactionGuid { get; }

        public decimal Amount { get; }

        public TransactionType TransactionType { get; }
        
        private int _transactionTypeId;

        public Guid CounterPartyUserGuid { get; private set; }


        private int _transactionStatusId;

        public TransactionStatus TransactionStatus { get; private set; }


    }
}