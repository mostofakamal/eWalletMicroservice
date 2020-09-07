using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transaction.Domain.SeedWork;

namespace Transaction.Domain.AggregateModel
{
    public interface IUserRepository : IRepository<User>
    {
        User Add(User user);

        Task<User> GetAsync(Guid userIdentityGuid);
        Task<User> GetAsync(string phoneNumber);
        Task<IList<PendingTransaction>> GetAllPendingTransactions();
        void AddPendingTransaction(PendingTransaction pendingTransaction);
    }
}