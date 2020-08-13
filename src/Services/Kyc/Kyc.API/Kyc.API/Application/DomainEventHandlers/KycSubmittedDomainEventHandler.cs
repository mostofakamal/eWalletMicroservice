using Kyc.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.API.Application.DomainEventHandlers
{
    public class KycSubmittedDomainEventHandler : INotificationHandler<KycSubmittedDomainEvent>
    {
        private readonly ILogger<KycSubmittedDomainEventHandler> logger;

        public KycSubmittedDomainEventHandler(ILogger<KycSubmittedDomainEventHandler> logger)
        {
            this.logger = logger;
        }

        public async Task Handle(KycSubmittedDomainEvent notification, CancellationToken cancellationToken)
        {

            this.logger.Log(LogLevel.Information, $"Value: {notification.FirstName}, {notification.LastName}");
        }
    }
}
