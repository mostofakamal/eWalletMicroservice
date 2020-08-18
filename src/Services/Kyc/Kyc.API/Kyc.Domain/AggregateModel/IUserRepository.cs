using Kyc.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kyc.Domain.AggregateModel
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Add(User user);
        Task<User> Get(Guid kycId);
        void Update(User order);
        
    }
}
