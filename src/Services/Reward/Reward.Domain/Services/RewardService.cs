using System;
using System.Threading.Tasks;
using Reward.Domain.AggregateModel;

namespace Core.Services
{
    public class RewardService : IRewardService
    {
        private readonly IUserRepository _repository;

        public RewardService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserReward> CheckAndProcessReward(User customer, RewardOperation operationName)
        {
            var eligibilityDetector = GetEligibilityDetector(operationName);
            var rewardRule = await eligibilityDetector.GetRewardRule();
            if (rewardRule != null)
            {
                var isCustomerEligibleForReward = await eligibilityDetector.IsEligible(customer);
                if (isCustomerEligibleForReward)
                {
                    var walletAdminCustomer =
                        await _repository.GetCountryAdminAsync(customer.CountryId);

                    if (walletAdminCustomer == null)
                    {
                        return null;
                    }

                    var userReward = new UserReward
                    {
                        User = customer,
                        RewardRule = rewardRule,
                        ReceivedOn = DateTime.UtcNow,
                        WalletUser = walletAdminCustomer
                    };

                    return userReward;
                }
            }
            return null;
        }

        private RewardEligibilityDetector GetEligibilityDetector(RewardOperation operationName)
        {
            if (operationName.Equals(RewardOperation.SubmitKyc)) return new KycRewardEligibilityDetector(_repository);
            if (operationName.Equals(RewardOperation.TransferMoney)) return new MoneyTransferRewardEligibilityDetector(_repository);
            if (operationName.Equals(RewardOperation.BillPayment)) return new BillPaymentRewardEligibilityDetector(_repository);

            throw new ArgumentException("operation name is Invalid");
        }
    }
}