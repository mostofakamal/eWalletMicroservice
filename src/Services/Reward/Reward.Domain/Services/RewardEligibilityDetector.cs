using System;
using System.Linq;
using System.Threading.Tasks;
using Reward.Domain.AggregateModel;

namespace Core.Services
{
    public abstract class RewardEligibilityDetector
    {
        private readonly IUserRepository _repository;
        protected RewardEligibilityDetector(IUserRepository repository)
        {
            _repository = repository;
        }

        public abstract RewardOperation OperationName { get; set; }

        public async Task<bool> IsEligible(User customer)
        {
            if (customer.IsCountryAdmin)
            {
                return false;
            }

            var hasAlreadyReceivedThisReward = customer.UserRewards
                .Any(x =>
                    x.RewardRule.Operation.Id == OperationName.Id);
            if (hasAlreadyReceivedThisReward)
            {
                return false;
            }

            var rewardRule = await GetRewardRule();
            return await CheckIfOperationDetailsMeetsRewardCriteria(customer, rewardRule);
        }

        public abstract Task<bool> CheckIfOperationDetailsMeetsRewardCriteria(User customer, RewardRule rewardRule);

        public async Task<RewardRule> GetRewardRule()
        {
            var allRules = await _repository.GetRewardRules();
            var rewardRule = allRules.FirstOrDefault(x =>
                x.OperationId == OperationName.Id
                && x.IsEnabled &&
                DateTime.UtcNow >= x.ValidFrom
                && DateTime.UtcNow <= x.ValidTo);
            return rewardRule;
        }
    }
}