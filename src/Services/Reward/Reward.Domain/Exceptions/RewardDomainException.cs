using System;

namespace Reward.Domain.Exceptions
{
    public class RewardDomainException : Exception
    {
        public RewardDomainException()
        { }

        public RewardDomainException(string message)
            : base(message)
        { }

        public RewardDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
