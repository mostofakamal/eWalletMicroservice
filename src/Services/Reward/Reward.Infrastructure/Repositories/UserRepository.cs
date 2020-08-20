using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Reward.Domain.AggregateModel;
using Reward.Domain.SeedWork;

namespace Reward.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly RewardContext _context;
        public UserRepository(RewardContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public User Add(User user)
        {
            return _context.Users.Add(user).Entity;
        }

        public async Task<User> GetAsync(string phoneNumber)
        {
            var user = await _context
                .Users
                .Include(x => x.UserRewards)
                //.ThenInclude(x => x.TransactionType)
                //.Include(x => x.Transactions).ThenInclude(x => x.TransactionStatus)
                .FirstOrDefaultAsync(o => o.PhoneNumber == phoneNumber);
            if (user == null)
            {
                user = _context
                    .Users
                    .Local
                    .FirstOrDefault(o => o.PhoneNumber == phoneNumber);
            }
            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(i => i.UserRewards).LoadAsync();

            }

            return user;
        }
        public async Task<User> GetAsync(Guid userIdentityGuid)
        {
            var user = await _context
                .Users
                .Include(x => x.UserRewards)
                //.ThenInclude(x=>x.TransactionType)
                //.Include(x => x.Transactions).ThenInclude(x => x.TransactionStatus)
                .FirstOrDefaultAsync(o => o.UserIdentityGuid == userIdentityGuid);
            if (user == null)
            {
                user = _context
                    .Users
                    .Local
                    .FirstOrDefault(o => o.UserIdentityGuid == userIdentityGuid);
            }
            if (user != null)
            {
                await _context.Entry(user)
                    .Collection(i => i.UserRewards).LoadAsync();
               
            }

            return user;
        }
    }
}
