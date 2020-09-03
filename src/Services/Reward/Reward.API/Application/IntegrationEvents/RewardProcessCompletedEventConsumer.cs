using System.Linq;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Reward.Domain.AggregateModel;

namespace Reward.API.Application.IntegrationEvents
{
    public class RewardProcessCompletedEventConsumer : IConsumer<RewardProcessCompletedEvent>
    {
        private readonly ILogger<RewardProcessCompletedEventConsumer> _logger;
        private readonly IUserRepository _userRepository;

        public RewardProcessCompletedEventConsumer(ILogger<RewardProcessCompletedEventConsumer> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<RewardProcessCompletedEvent> context)
        {
            var rewardProcessCompletedEventMessage = context.Message;
            _logger.LogInformation($"Consuming RewardProcessCompletedEvent with correlationId: {rewardProcessCompletedEventMessage.CorrelationId}");
            var user = await _userRepository.GetAsync(rewardProcessCompletedEventMessage.RewardReceiverGuid);
            user.UpdateUserRewardStatusForReward(rewardProcessCompletedEventMessage.CorrelationId,UserRewardStatus.PaidOut);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}