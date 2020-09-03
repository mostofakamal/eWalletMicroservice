using Reward.Domain.SeedWork;
using System;

namespace Reward.Domain.AggregateModel
{
    public class UserReward : Entity
    {
        public UserReward()
        {
        }

        public UserReward(User user, User walletUser, RewardRule rewardRule)
        {
            this.User = user;
            this.WalletUser = walletUser;
            this.RewardRule = rewardRule;
            ReceivedOn = DateTime.UtcNow;
            RewardGuid = Guid.NewGuid();
            _statusId = UserRewardStatus.Pending.Id;
        }
        public int UserId { get; private set; }

        public User User { get; private set; }

        public int WalletUserId { get; private set; }
        public User WalletUser { get; private set; }

        public int RewardRuleId { get; private set; }

        public RewardRule RewardRule { get; private set; }

        public DateTime ReceivedOn { get; private set; }

       

        public Guid RewardGuid { get; private set; }

        private int _statusId;
        public UserRewardStatus Status { get; private set; }

        public void UpdateStatus(UserRewardStatus newStatus)
        {
            _statusId = newStatus.Id;
        }
    }
}
