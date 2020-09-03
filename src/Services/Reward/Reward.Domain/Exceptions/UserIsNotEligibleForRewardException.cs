using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Exceptions
{
    public class UserIsNotEligibleForRewardException : RewardDomainException
    {
        public UserIsNotEligibleForRewardException(string message): base(message)
        {

        }
    }
}
