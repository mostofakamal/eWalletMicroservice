using System;

namespace Reward.Domain.AggregateModel
{
    public class UserReward
    {
        public UserReward()
        {
            RewardGuid = Guid.NewGuid();
            _statusId = UserRewardStatus.Pending.Id;
        }
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int WalletUserId { get; set; }
        public User WalletUser { get; set; }

        public int RewardRuleId { get; set; }

        public RewardRule RewardRule { get; set; }

        public DateTime ReceivedOn { get; set; }

        public Guid RewardGuid { get; private set; }

        private int _statusId;
        public UserRewardStatus Status { get; private set; }

        public void UpdateStatus(UserRewardStatus newStatus)
        {
            _statusId = newStatus.Id;
        }
    }
}
