using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.Domain.Events
{
    public class KycVerifiedDomainEvent : INotification
    {
        public Guid UserId { get; set; }
    }
}
