using System;
using System.Collections.Generic;
using System.Linq;
using Reward.Domain.SeedWork;

namespace Reward.Domain.AggregateModel
{
    public class User : Entity, IAggregateRoot
    {
        public Guid UserIdentityGuid { get; private set; }

        public int CountryId { get; private set; }

        public string PhoneNumber { get; private set; }

        public List<Reward> Rewards { get; private set;}

        protected User()
        {
        }

        public User(Guid userId, int countryId, string phoneNumber)
        {
            UserIdentityGuid = userId;
            CountryId = countryId;
            PhoneNumber = phoneNumber;
        }

    }
}
