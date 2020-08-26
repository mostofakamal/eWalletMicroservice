using MediatR;
using System;

namespace Reward.Domain.Events
{
    public class KycApprovedDomainEvent : INotification
    {
        public Guid UserId { get; set; }
    }
}
