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

        public async Task<User> Add(User user)
        {
            var added = await _context.Users.AddAsync(user);
            return added.Entity;
        }

        public async Task<User> Get(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Country)
                .Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user != null)
                await _context.Entry(user)
                    .Collection(k => k.KycInformations).LoadAsync();

            return user;
        }

        public void Update(User user)
        {
            foreach (var item in user.KycInformations)
            {
                if (Guid.Empty == item.Id)
                    _context.Entry(item).State = EntityState.Added;
                else
                    _context.Attach(item).State = EntityState.Modified;
            }
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
