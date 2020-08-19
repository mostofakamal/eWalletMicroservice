using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.SeedWork;

namespace Transaction.Infrastructure.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly TransactionContext _context;
        public UserRepository(TransactionContext context)
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
                .Include(x => x.Transactions).ThenInclude(x => x.TransactionType)
                .Include(x => x.Transactions).ThenInclude(x => x.TransactionStatus)
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
                    .Collection(i => i.Transactions).LoadAsync();

            }

            return user;
        }
        public async Task<User> GetAsync(Guid userIdentityGuid)
        {
            var user = await _context
                .Users
                .Include(x => x.Transactions).ThenInclude(x=>x.TransactionType)
                .Include(x => x.Transactions).ThenInclude(x => x.TransactionStatus)
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
                    .Collection(i => i.Transactions).LoadAsync();
               
            }

            return user;
        }
    }
}
