using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MediatR;
using Transaction.API.Application.IntegrationEvents;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Events;

namespace Transaction.API.Application.DomainEventHandlers
{
    public class DebitTransactionCreatedDomainEventHandler: INotificationHandler<DebitTransactionCreatedDomainEvent>
    {
        private readonly ITransactionIntegrationDataService _transactionIntegrationDataService;
        private readonly IUserRepository _userRepository;

        public DebitTransactionCreatedDomainEventHandler(ITransactionIntegrationDataService transactionIntegrationDataService, IUserRepository userRepository)
        {
            _transactionIntegrationDataService = transactionIntegrationDataService;
            _userRepository = userRepository;
        }

        public async Task Handle(DebitTransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var debitIntegrationEvent = new DebitTransactionCreatedIntegrationEvent(notification.Amount,
                notification.TransactionGuid, notification.SenderUserGuid, notification.ReceiverUserGuid,
                notification.TransactionType.Name,notification.CorrelationId);

            var user = await _userRepository.GetAsync(notification.SenderUserGuid);
            var smsText =
                $"Your account has been debited by amount: {notification.Amount} for {notification.TransactionType.Name}. Your current account Balance: {user.GetBalance()}";
            var smsMessage = new SmsIntegrationMessage(user.PhoneNumber, smsText);
            await _transactionIntegrationDataService.AddAndSaveAsync(new List<IntegrationData>()
            {
                debitIntegrationEvent,smsMessage
            });
        }
    }
}
