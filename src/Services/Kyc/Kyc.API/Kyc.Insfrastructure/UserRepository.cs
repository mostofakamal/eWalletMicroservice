using Kyc.Domain.AggregateModel;
using Kyc.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.Insfrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;

        public IUnitOfWork UnitOfWork { get { return _context; } }


        public UserRepository(UserContext kycContext)
        {
            _context = kycContext;
        }

        public async Task<KycInformation> Add(KycInformation kycInformation)
        {
            var added = await _context.Kycs.AddAsync(kycInformation);
            return added.Entity;
        }

        public async Task<User> Add(User user)
        {
            var added = await _context.Users.AddAsync(user);
            return added.Entity;
        }

        public async Task<User> Get(Guid userId)
        {
            return await _context.Users
                .Include(k => k.KycInformations)
                .Include(u => u.Country)
                .Where(x => x.Id == userId).FirstOrDefaultAsync();
        }
    }
}
