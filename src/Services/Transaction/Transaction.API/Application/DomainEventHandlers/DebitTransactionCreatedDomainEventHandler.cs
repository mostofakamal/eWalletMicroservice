using System.Threading;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MediatR;
using Transaction.API.Application.IntegrationEvents;
using Transaction.Domain.Events;

namespace Transaction.API.Application.DomainEventHandlers
{
    public class DebitTransactionCreatedDomainEventHandler: INotificationHandler<DebitTransactionCreatedDomainEvent>
    {
        private readonly ITransactionIntegrationEventService _transactionIntegrationEventService;

        public DebitTransactionCreatedDomainEventHandler(ITransactionIntegrationEventService transactionIntegrationEventService)
        {
            _transactionIntegrationEventService = transactionIntegrationEventService;
        }

        public async Task Handle(DebitTransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var debitIntegrationEvent = new DebitTransactionCreatedIntegrationEvent(notification.Amount,
                notification.TransactionGuid, notification.SenderUserGuid, notification.ReceiverUserGuid,
                notification.TransactionType.Name);
            await _transactionIntegrationEventService.AddAndSaveEventAsync(debitIntegrationEvent);
        }
    }
}
