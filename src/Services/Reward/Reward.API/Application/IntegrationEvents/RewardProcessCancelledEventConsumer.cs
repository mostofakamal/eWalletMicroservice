using System.Linq;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Reward.Domain.AggregateModel;

namespace Reward.API.Application.IntegrationEvents
{
    public class RewardProcessCancelledEventConsumer : IConsumer<RewardProcessCancelledEvent>
    {
        private readonly ILogger<RewardProcessCancelledEventConsumer> _logger;
        private readonly IUserRepository _userRepository;

        public RewardProcessCancelledEventConsumer(ILogger<RewardProcessCancelledEventConsumer> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<RewardProcessCancelledEvent> context)
        {
            var rewardProcessCancelledEventMessage = context.Message;
            _logger.LogInformation($"Consuming {nameof(RewardProcessCancelledEvent)} with correlationId: {rewardProcessCancelledEventMessage.CorrelationId} and FailedReason: {rewardProcessCancelledEventMessage.Reason}");
            var user= await _userRepository.GetAsync(rewardProcessCancelledEventMessage.ReceiverUserGuid);
            user.UpdateUserRewardStatusForReward(rewardProcessCancelledEventMessage.CorrelationId, UserRewardStatus.TransactionFailed);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}