using System.Threading.Tasks;
using Reward.Domain.AggregateModel;

namespace Core.Services
{
    public abstract class TransactionalRewardEligibilityDetector : RewardEligibilityDetector
    {
        private readonly IUserRepository _repository;
        protected TransactionalRewardEligibilityDetector(IUserRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public abstract TransactionType TransactionType { get; set; }
        public override async Task<bool> CheckIfOperationDetailsMeetsRewardCriteria(User customer, RewardRule rewardRule)
        {
            //if (TransactionType.Transfer.Equals(TransactionType))
            //    return customer.TransactionCount == rewardRule.RequiredMinOccurance;
            //if (TransactionType.BillPayment.Equals(TransactionType))
            //    return customer.TransactionCount == rewardRule.RequiredMinOccurance;
            
            ///TODO:What will be the other cases;
            return true;
        }
    }
}