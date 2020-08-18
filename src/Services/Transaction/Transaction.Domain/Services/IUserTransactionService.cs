using System;
using System.Threading.Tasks;
using Transaction.Domain.AggregateModel;

namespace Transaction.Domain.Services
{
    public interface IUserTransactionService
    {
        Task DoTransaction(decimal amount, Guid senderUserGuid, Guid receiverUserGuid,
            TransactionType transactionType);
    }
}