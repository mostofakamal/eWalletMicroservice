using System;

namespace Reward.Domain.AggregateModel
{
    public class UserReward
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int RewardRuleId { get; set; }

        public RewardRule RewardRule { get; set; }

        public DateTime ReceivedOn { get; set; }
    }
}
