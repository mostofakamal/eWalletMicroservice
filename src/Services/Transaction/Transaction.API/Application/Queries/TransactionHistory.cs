using System;

namespace Transaction.API.Application.Queries
{
    public class TransactionHistory
    {
        public decimal Amount { get; set; }

        public Guid TransactionId { get; set; }

        public string Kind => Amount < 0 ? "Debit" : "Credit";

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public Guid CounterPartyUserGuid { get; set; }

        public DateTime Date { get; set; }

    }
}