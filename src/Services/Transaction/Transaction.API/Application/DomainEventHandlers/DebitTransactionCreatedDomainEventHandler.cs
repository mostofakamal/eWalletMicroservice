using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transaction.Domain.Events;

namespace Transaction.API.Application.DomainEventHandlers
{
    public class DebitTransactionCreatedDomainEventHandler: INotificationHandler<DebitTransactionCreatedDomainEvent>
    {
        public Task Handle(DebitTransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
           return Task.CompletedTask;
        }
    }
}
