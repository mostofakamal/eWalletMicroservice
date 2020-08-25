using System.Threading.Tasks;
using Reward.Domain.AggregateModel;

namespace Core.Services
{
    public class KycRewardEligibilityDetector: RewardEligibilityDetector
    {
        private readonly IUserRepository _repository;

        public KycRewardEligibilityDetector(IUserRepository repository): base(repository)
        {
            _repository = repository;
        }

        public override RewardOperation OperationName { get; set; } = RewardOperation.SubmitKyc;

        public override Task<bool> CheckIfOperationDetailsMeetsRewardCriteria(User customer, RewardRule rewardRule)
        {
            return Task.FromResult(customer.IsTransactionEligible);
        }
    }
}