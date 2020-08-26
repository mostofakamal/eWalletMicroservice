using Reward.Domain.AggregateModel;

namespace Core.Services
{
    public class BillPaymentRewardEligibilityDetector : TransactionalRewardEligibilityDetector
    {
        public BillPaymentRewardEligibilityDetector(IUserRepository repository) : base(repository)
        {
        }

        public override RewardOperation OperationName { get; set; } = RewardOperation.BillPayment;

        public override TransactionType TransactionType { get; set; } = TransactionType.BillPayment;
    }
}