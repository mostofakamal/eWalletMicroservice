using Reward.Domain.AggregateModel;

namespace Core.Services
{
    public class MoneyTransferRewardEligibilityDetector: TransactionalRewardEligibilityDetector
    {
        public MoneyTransferRewardEligibilityDetector(IUserRepository repository): base(repository)
        {
        }

        public override TransactionType TransactionType { get; set; } = TransactionType.Transfer;

        public override RewardOperation OperationName { get; set; } = RewardOperation.TransferMoney;

    }
}