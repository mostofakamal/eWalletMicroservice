using Core.Lib.IntegrationEvents;
using Core.Lib.RabbitMq;
using Core.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Reward.Domain.AggregateModel;
using Reward.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reward.API.Application.DomainEventHandlers
{
    public class KycApprovedDomainEventHandler : INotificationHandler<KycApprovedRewardProcessingDomainEvent>
    {
        private readonly IUserRepository userRepository;
        private readonly IRewardService rewardService;
        private readonly IBus bus;
        private readonly ILogger<KycApprovedDomainEventHandler> logger;

        public KycApprovedDomainEventHandler(IUserRepository userRepository,
            IRewardService rewardService,
            IBus bus,
            ILogger<KycApprovedDomainEventHandler> logger)
        {
            this.userRepository = userRepository;
            this.rewardService = rewardService;
            this.bus = bus;
            this.logger = logger;
        }

        public async Task Handle(KycApprovedRewardProcessingDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(notification.UserId);
            var userReward = await this.rewardService.CheckAndProcessReward(user, RewardOperation.SubmitKyc);
            if (userReward != null)
            {
                user.UserRewards.Add(userReward);
                await userRepository.UnitOfWork.SaveChangesAsync();
                var transactionIntegrationMessage = new TransactionIntegrationMessage()
                {
                    Amount = userReward.RewardRule.Amount,
                    ReceiverUserGuid = user.UserIdentityGuid,
                    SenderUserGuid = userReward.WalletUser.UserIdentityGuid,
                    TransactionType = TransactionType.Reward.Id
                };
                await bus.Send<TransactionIntegrationMessage>(transactionIntegrationMessage);
            }
        }
    }
}
