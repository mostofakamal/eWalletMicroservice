using Core.Lib.IntegrationEvents;
using MediatR;
using Reward.API.Application.IntegrationEvents;
using Reward.Domain.AggregateModel;
using Reward.Domain.Events;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Reward.API.Application.DomainEventHandlers
{
    public class UserRewardAddedDomainEventHandler : INotificationHandler<UserRewardAddedDomainEvent>
    {
        private readonly IRewardIntegrationDataService _transactionIntegrationDataService;
        private readonly ILogger<UserRewardAddedDomainEventHandler> _logger;

        public UserRewardAddedDomainEventHandler(IRewardIntegrationDataService transactionIntegrationDataService, ILogger<UserRewardAddedDomainEventHandler> logger)
        {
            this._transactionIntegrationDataService = transactionIntegrationDataService;
            _logger = logger;
        }

        public async Task Handle(UserRewardAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var rewardProcessStarted = new RewardProcessStartedEvent(notification.Amount, notification.UserId, notification.CountryAdminId, TransactionType.Reward.Id, notification.UserRewardGuid);
            _logger.LogInformation($"Saving rewardProcessStarted event with correlation Id: {rewardProcessStarted.CorrelationId} Amount: {rewardProcessStarted.Amount} Sender: {rewardProcessStarted.SenderUserGuid} Receiver : {rewardProcessStarted.ReceiverUserGuid}");
            await this._transactionIntegrationDataService.AddAndSaveAsync(rewardProcessStarted);
        }
    }
}
