using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Reward.Domain.Events;
using Reward.Domain.SeedWork;

namespace Reward.Domain.AggregateModel
{
    public class User : Entity, IAggregateRoot
    {
        public Guid UserIdentityGuid { get; private set; }

        public int CountryId { get; private set; }

        public string PhoneNumber { get; private set; }

        public bool IsTransactionEligible { get; private set; }

        public bool IsCountryAdmin { get; private set; }

        //[NotMapped]
        //public int TransactionCount { get; set; }

        //[NotMapped]
        //public int TransferMoneyCount { get; set; }

        public List<UserReward> UserRewards { get; private set; }

        protected User()
        {
        }

        public User(Guid userId, int countryId, string phoneNumber, bool isTransactionEligible = false, bool isCountryAdmin = false)
        {
            UserIdentityGuid = userId;
            CountryId = countryId;
            PhoneNumber = phoneNumber;
            IsTransactionEligible = isTransactionEligible;
            IsCountryAdmin = isCountryAdmin;
        }

        public void SetUserTransactionEligible()
        {
            this.IsTransactionEligible = true;
            AddDomainEvent(new KycApprovedRewardProcessingDomainEvent() { UserId = this.UserIdentityGuid });
        }
    }
}
