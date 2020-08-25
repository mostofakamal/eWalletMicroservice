using Core.Lib.IntegrationEvents;
using Kyc.API.Application.IntegrationEvents;
using Kyc.Domain.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.API.Application.DomainEventHandlers
{
    public class KycVerifiedDomainEventHandler : INotificationHandler<KycVerifiedDomainEvent>
    {
        private readonly IKycIntegrationDataService kycIntegrationDataService;

        public KycVerifiedDomainEventHandler(IKycIntegrationDataService kycIntegrationDataService)
        {
            this.kycIntegrationDataService = kycIntegrationDataService;
        }

        public async Task Handle(KycVerifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            var kycApproved = new KycApprovedIntegrationEvent
            {
                UserId = notification.UserId
            };
            await this.kycIntegrationDataService.AddAndSaveAsync(kycApproved);
        }
    }
}
