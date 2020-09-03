using Core.Lib.IntegrationEvents;
using Core.Lib.RabbitMq;
using Core.Services;
using MassTransit;
using MediatR;
using Reward.Domain.AggregateModel;
using Reward.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Reward.API.Application.DomainEventHandlers
{
    public class KycApprovedDomainEventHandler : INotificationHandler<KycApprovedDomainEvent>
    {
        private readonly IUserRepository userRepository;
        private readonly IRewardService rewardService;
        private readonly IBus bus;
        private readonly ILogger<KycApprovedDomainEventHandler> _logger;

        public KycApprovedDomainEventHandler(IUserRepository userRepository,
            IRewardService rewardService,
            IBus bus, ILogger<KycApprovedDomainEventHandler> logger)
        {
            this.userRepository = userRepository;
            this.rewardService = rewardService;
            this.bus = bus;
            _logger = logger;
        }

        public async Task Handle(KycApprovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(notification.UserId);
            var userReward = await this.rewardService.CheckAndProcessReward(user, RewardOperation.SubmitKyc);
            if (userReward != default)
            {
                user.UserRewards.Add(userReward);
                await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                //var transactionIntegrationMessage = new TransactionIntegrationMessage()
                //{
                //    Amount = userReward.RewardRule.Amount,
                //    ReceiverUserGuid = user.UserIdentityGuid,
                //    SenderUserGuid = userReward.WalletUser.UserIdentityGuid,
                //    TransactionType = TransactionType.Reward.Id
                //};

                var rewardProcessStarted = new RewardProcessStartedEvent(userReward.RewardRule.Amount, userReward.WalletUser.UserIdentityGuid, user.UserIdentityGuid, TransactionType.Reward.Id, userReward.RewardGuid);
                // await bus.Send<TransactionIntegrationMessage>(transactionIntegrationMessage);
                _logger.LogInformation($"publishing rewardProcessStartedevent with correlation Id: {rewardProcessStarted.CorrelationId} Amount: {rewardProcessStarted.Amount} Sender: {rewardProcessStarted.SenderUserGuid} Receiver : {rewardProcessStarted.ReceiverUserGuid}");
                 await bus.Publish(rewardProcessStarted, cancellationToken: cancellationToken);
            }
        }
    }
}
