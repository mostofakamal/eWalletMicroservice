using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using Core.Services;
using Microsoft.Extensions.Logging;
using Reward.Domain.AggregateModel;

namespace Reward.API.Application.Commands
{
    public class ProcessRewardForKycHandler : IRequestHandler<ProcessRewardForKyc, bool>
    {
        private readonly ILogger<KycApprovedIntegrationEvent> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IRewardService rewardService;

        public ProcessRewardForKycHandler(ILogger<KycApprovedIntegrationEvent> logger,
            IUserRepository userRepository,
            IRewardService rewardService)
        {
            _logger = logger;
            _userRepository = userRepository;
            this.rewardService = rewardService;
        }
        public async Task<bool> Handle(ProcessRewardForKyc request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(request.UserId);
            if (user != null)
            {
                user.EnableTransactionEligibility();
                var rewardRule = await this.rewardService.GetRewardRule(user, RewardOperation.SubmitKyc);
                var countryAdmin = await this.rewardService.GetCountryAdmin(user);
                user.AddUserReward(rewardRule, countryAdmin);
                return await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            }
            else
            {
                _logger.LogWarning($"User with Id:  {request.UserId} does not exist in reward service");
                return false;
            }
        }
    }
}
