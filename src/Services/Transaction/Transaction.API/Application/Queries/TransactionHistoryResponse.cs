using System.Collections.Generic;

namespace Transaction.API.Application.Queries
{
    public class TransactionHistoryResponse
    {
        public IList<TransactionHistory> TransactionHistories { get; set; }
    }
}