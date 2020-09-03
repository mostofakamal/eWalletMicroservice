using MediatR;
using Reward.Domain.AggregateModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reward.Domain.Events
{
    public class UserRewardAddedDomainEvent : INotification
    {
        public Guid UserId { get; set; }
        public Guid CountryAdminId { get; set; }
        public decimal Amount { get; set; }

        public UserRewardAddedDomainEvent(Guid userId, Guid countryAdminId, decimal amount)
        {
            UserId = userId;
            CountryAdminId = countryAdminId;
            Amount = amount;
        }
    }
}
