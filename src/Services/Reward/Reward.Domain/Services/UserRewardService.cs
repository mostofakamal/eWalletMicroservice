using System;
using System.Threading.Tasks;
using Reward.Domain.AggregateModel;
using Reward.Domain.Exceptions;

namespace Reward.Domain.Services
{
    public class UserRewardService : IUserRewardService
    {
        private readonly IUserRepository _userRepository;

        public UserRewardService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
