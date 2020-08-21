using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transaction.Domain.Events;

namespace Transaction.API.Application.DomainEventHandlers
{
    public class CreditTransactionCreatedDomainEventHandler : INotificationHandler<CreditTransactionCreatedDomainEvent>
    {
        public Task Handle(CreditTransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask; 
        }
    }

}