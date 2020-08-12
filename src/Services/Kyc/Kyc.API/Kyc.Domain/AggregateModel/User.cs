using Kyc.Domain.SeedWork;
using System;
using System.Runtime.CompilerServices;

namespace Kyc.Domain.AggregateModel
{
    public class User : Entity, IAggregateRoot
    {
        public bool IsKycVerified { get; set; }

        public User()
        {

        }
        public User(Guid userId, bool isKycVerified)
        {
            this.Id = userId;
            this.IsKycVerified = isKycVerified;
        }
    }
}
