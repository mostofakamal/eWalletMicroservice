using Core.Lib.IntegrationEvents;
using Kyc.API.Application.Services;
using Kyc.Domain.AggregateModel;
using Kyc.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;

namespace Kyc.API.Application.DomainEventHandlers
{
    public class KycSubmittedDomainEventHandler : INotificationHandler<KycSubmittedDomainEvent>
    {
        private readonly ILogger<KycSubmittedDomainEventHandler> logger;
        private readonly IExternalKycVerifier externalKycVerifier;
        private readonly IKycRepository kycRepository;
        private readonly IPublishEndpoint endpoint;
        private readonly IIdentityService identityService;

        public KycSubmittedDomainEventHandler(ILogger<KycSubmittedDomainEventHandler> logger,
            IExternalKycVerifier externalKycVerifier,
            IKycRepository kycRepository,
            IPublishEndpoint endpoint,
            IIdentityService identityService)
        {
            this.logger = logger;
            this.externalKycVerifier = externalKycVerifier;
            this.kycRepository = kycRepository;
            this.endpoint = endpoint;
            this.identityService = identityService;
        }

        public async Task Handle(KycSubmittedDomainEvent notification, CancellationToken cancellationToken)
        {
            var kycRequest = new KycVerificationRequest() { FirstName = notification.FirstName, LastName = notification.LastName, NID = notification.NID };
            this.logger.Log(LogLevel.Information, $"Value: {notification.FirstName}, {notification.LastName} {notification.NID}");

            var kycVerificationResult = await externalKycVerifier.Verify(kycRequest, "Bangladesh");

            this.logger.Log(LogLevel.Information, $"Value:{notification.NID}, {notification.FirstName}, {notification.LastName}, {kycVerificationResult} ");

            var kyc = await this.kycRepository.Get(notification.KycId);
            kyc.KycStatusId = (short)kycVerificationResult;

            if (kycVerificationResult == KycStatuses.Approved)
            {
                var userId = identityService.GetUserIdentity();
                await endpoint.Publish<IKycApprovedEvent>(new KycApprovedEvent() { UserId = Guid.Parse(userId) });
            }

            await this.kycRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}
