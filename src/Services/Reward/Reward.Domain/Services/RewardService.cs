using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Reward.Domain.AggregateModel;
using Reward.Domain.Exceptions;

namespace Core.Services
{
    public class RewardService : IRewardService
    {
        private readonly IUserRepository _repository;

        public RewardService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<RewardRule> GetRewardRule(User user, RewardOperation operationName)
        {
            var eligibilityDetector = GetEligibilityDetector(operationName);
            var rewardRule = await eligibilityDetector.GetRewardRule();

            var operation = rewardRule.Operation;

            if (rewardRule == null) throw new RewardNotConfiguredException($"Reward is not configured for {operation.Name}");

            var isCustomerEligibleForReward = await eligibilityDetector.IsEligible(user);

            if (!isCustomerEligibleForReward)
            {
                throw new UserIsNotEligibleForRewardException($"User is not eligible for reward for {operation.Name}");
            }

            return rewardRule;
        }

        public async Task<User> GetCountryAdmin(User user)
        {
            var walletAdminCustomer = await _repository.GetCountryAdminAsync(user.CountryId);

            if (walletAdminCustomer == null) throw new RewardNotConfiguredException($"Country admin is not configured");

            return walletAdminCustomer;
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