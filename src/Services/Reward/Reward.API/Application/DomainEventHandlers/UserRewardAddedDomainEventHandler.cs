using Core.Lib.IntegrationEvents;
using MediatR;
using Reward.API.Application.IntegrationEvents;
using Reward.Domain.AggregateModel;
using Reward.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reward.API.Application.DomainEventHandlers
{
    public class UserRewardAddedDomainEventHandler : INotificationHandler<UserRewardAddedDomainEvent>
    {
        private readonly IRewardIntegrationDataService transactionIntegrationDataService;

        public UserRewardAddedDomainEventHandler(IRewardIntegrationDataService transactionIntegrationDataService)
        {
            this.transactionIntegrationDataService = transactionIntegrationDataService;
        }

        public async Task Handle(UserRewardAddedDomainEvent notification, CancellationToken cancellationToken)
        {
            var transactionIntegrationMessage = new TransactionIntegrationMessage()
            {
                Amount = notification.Amount,
                ReceiverUserGuid = notification.UserId,
                SenderUserGuid = notification.CountryAdminId,
                TransactionType = TransactionType.Reward.Id
            };
            await this.transactionIntegrationDataService.AddAndSaveAsync(transactionIntegrationMessage);
        }
    }
}
