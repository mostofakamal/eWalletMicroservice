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
        private readonly ITransactionIntegrationDataService _transactionIntegrationDataService;

        public DebitTransactionCreatedDomainEventHandler(ITransactionIntegrationDataService transactionIntegrationDataService)
        {
            _transactionIntegrationDataService = transactionIntegrationDataService;
        }

        public async Task Handle(DebitTransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var debitIntegrationEvent = new DebitTransactionCreatedIntegrationEvent(notification.Amount,
                notification.TransactionGuid, notification.SenderUserGuid, notification.ReceiverUserGuid,
                notification.TransactionType.Name);
            await _transactionIntegrationDataService.AddAndSaveAsync(debitIntegrationEvent);
        }
    }
}
