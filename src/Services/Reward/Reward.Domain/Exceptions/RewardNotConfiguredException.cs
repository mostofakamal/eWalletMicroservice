using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Exceptions
{
    public class RewardNotConfiguredException: RewardDomainException
    {
        public RewardNotConfiguredException(string message) : base(message)
        {

        }
    }
}
