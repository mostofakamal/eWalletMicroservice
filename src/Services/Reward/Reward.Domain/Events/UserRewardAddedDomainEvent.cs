using MediatR;
using Reward.Domain.AggregateModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Events
{
    public class UserRewardAddedDomainEvent : INotification
    {
        public Guid UserId { get; private set; }
        public Guid CountryAdminId { get; private set; }
        public decimal Amount { get; private set; }

        public Guid UserRewardGuid { get; private set; }

        public UserRewardAddedDomainEvent(Guid userId, Guid countryAdminId, decimal amount,Guid rewardGuid)
        {
            UserId = userId;
            CountryAdminId = countryAdminId;
            Amount = amount;
            UserRewardGuid = rewardGuid;
        }
    }
}
