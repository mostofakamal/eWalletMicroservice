using Kyc.Domain.SeedWork;
using System;

namespace Kyc.Domain.AggregateModel
{
    public class User : Entity, IAggregateRoot
    {
        public bool IsKycVerified { get; set; }
    }
}
