using Kyc.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.API.Application.DomainEventHandlers
{
    public class KycSubmittedDomainEventHandler : INotificationHandler<KycStartedDomainEvent>
    {
        private readonly ILogger<KycSubmittedDomainEventHandler> logger;

        public KycSubmittedDomainEventHandler(ILogger<KycSubmittedDomainEventHandler> logger)
        {
            this.logger = logger;
        }
        public async Task Handle(KycStartedDomainEvent notification, CancellationToken cancellationToken)
        {
            this.logger.Log(LogLevel.Information, $"Value: {notification.FirstName}, {notification.LastName}");
        }
    }
}
