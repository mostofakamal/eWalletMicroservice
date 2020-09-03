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

        public IEnumerable<UserReward> UserRewards => _userRewards;

        private readonly List<UserReward> _userRewards;

        protected User()
        {
            _userRewards = new List<UserReward>();
        }

        public User(Guid userId, int countryId, string phoneNumber, bool isTransactionEligible = false, bool isCountryAdmin = false)
        {
            UserIdentityGuid = userId;
            CountryId = countryId;
            PhoneNumber = phoneNumber;
            IsTransactionEligible = isTransactionEligible;
            IsCountryAdmin = isCountryAdmin;
        }

        public void EnableTransactionEligibility()
        {
            this.IsTransactionEligible = true;
        }

        public void AddUserReward(RewardRule rewardRule, User countryAdmin)
        {
            var userReward = new UserReward(this,countryAdmin,rewardRule);
            _userRewards.Add(userReward);
            var userRewardDomainEvent = new UserRewardAddedDomainEvent(userReward.User.UserIdentityGuid,
                userReward.WalletUser.UserIdentityGuid, userReward.RewardRule.Amount);
            AddDomainEvent(userRewardDomainEvent);
        }
    }
}
