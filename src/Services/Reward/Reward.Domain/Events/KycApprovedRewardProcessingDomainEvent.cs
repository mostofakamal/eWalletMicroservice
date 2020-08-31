using MediatR;
using System;

namespace Reward.Domain.Events
{
    public class KycApprovedRewardProcessingDomainEvent : INotification
    {
        public Guid UserId { get; set; }

    }
}
