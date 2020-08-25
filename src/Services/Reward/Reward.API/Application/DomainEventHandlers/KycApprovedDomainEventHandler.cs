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

namespace Reward.API.Application.DomainEventHandlers
{
    public class KycApprovedDomainEventHandler : INotificationHandler<KycApprovedDomainEvent>
    {
        private readonly IUserRepository userRepository;
        private readonly IRewardService rewardService;
        private readonly IBus bus;

        public KycApprovedDomainEventHandler(IUserRepository userRepository,
            IRewardService rewardService,
            IBus bus)
        {
            this.userRepository = userRepository;
            this.rewardService = rewardService;
            this.bus = bus;
        }

        public async Task Handle(KycApprovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(notification.UserId);
            var userReward = await this.rewardService.CheckAndProcessReward(user, RewardOperation.SubmitKyc);
            if (userReward != default)
            {
                user.UserRewards.Add(userReward);
                await userRepository.UnitOfWork.SaveChangesAsync();
                var transactionIntegrationMessage = new TransactionIntegrationMessage()
                {
                    Amount = userReward.RewardRule.Amount,
                    ReceiverUserGuid = user.UserIdentityGuid,
                    SenderUserGuid = user.UserIdentityGuid,
                    TransactionType = TransactionType.Reward.Id
                };
                await bus.Send<TransactionIntegrationMessage>(transactionIntegrationMessage);
            }
        }
    }
}
