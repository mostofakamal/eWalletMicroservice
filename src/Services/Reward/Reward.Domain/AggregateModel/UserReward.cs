using Reward.Domain.SeedWork;
using System;

namespace Reward.Domain.AggregateModel
{
    public class UserReward : Entity
    {
        public int UserId { get; private set; }

        public User User { get; private set; }

        public int WalletUserId { get; private set; }
        public User WalletUser { get; private set; }

        public int RewardRuleId { get; private set; }

        public RewardRule RewardRule { get; private set; }

        public DateTime ReceivedOn { get; private set; }

        public UserReward()
        {

        }

        public UserReward(User user, User walletUser, RewardRule rewardRule)
        {
            this.User = user;
            this.WalletUser = walletUser;
            this.RewardRule = rewardRule;
            ReceivedOn = DateTime.UtcNow;
        }
    }
}
