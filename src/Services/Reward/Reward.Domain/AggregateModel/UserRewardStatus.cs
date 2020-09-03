using System;
using System.Collections.Generic;
using System.Linq;
using Reward.Domain.Exceptions;
using Reward.Domain.SeedWork;

namespace Reward.Domain.AggregateModel
{
    public class UserRewardStatus : Enumeration
    {
        public static UserRewardStatus Pending = new UserRewardStatus(1, nameof(Pending));
        public static UserRewardStatus PaidOut = new UserRewardStatus(2, nameof(PaidOut));
        public static UserRewardStatus TransactionFailed = new UserRewardStatus(3, nameof(TransactionFailed));

        public UserRewardStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<UserRewardStatus> List() =>
            new[] { Pending, PaidOut, TransactionFailed };

        public static UserRewardStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new RewardDomainException($"Possible values for UserRewardStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static UserRewardStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new RewardDomainException($"Possible values for UserRewardStatus: {string.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}