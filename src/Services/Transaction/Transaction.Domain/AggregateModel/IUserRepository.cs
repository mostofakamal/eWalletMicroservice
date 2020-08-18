using System;
using System.Threading.Tasks;
using Transaction.Domain.SeedWork;

namespace Transaction.Domain.AggregateModel
{
    public interface IUserRepository : IRepository<User>
    {
        User Add(User user);

        Task<User> GetAsync(Guid userIdentityGuid);
    }
}