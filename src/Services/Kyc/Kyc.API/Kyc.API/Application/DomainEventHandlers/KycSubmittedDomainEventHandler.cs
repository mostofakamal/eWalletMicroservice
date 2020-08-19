using Core.Lib.IntegrationEvents;
using Kyc.API.Application.Services;
using Kyc.Domain.AggregateModel;
using Kyc.Domain.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;
using Kyc.API.Application.IntegrationEvents;
using System.Collections.Generic;

namespace Kyc.API.Application.DomainEventHandlers
{
    public class KycSubmittedDomainEventHandler : INotificationHandler<KycSubmittedDomainEvent>
    {
        private readonly ILogger<KycSubmittedDomainEventHandler> logger;
        private readonly IExternalKycVerifier externalKycVerifier;
        private readonly IUserRepository userRepository;
        private readonly IIdentityService identityService;
        private readonly KycApprovedEventPublisher eventPublisher;
        private readonly IntegrationEventService integrationEventService;

        public KycSubmittedDomainEventHandler(ILogger<KycSubmittedDomainEventHandler> logger,
            IExternalKycVerifier externalKycVerifier,
            IUserRepository userRepository,
            IPublishEndpoint endpoint,
            IIdentityService identityService,
            KycApprovedEventPublisher eventPublisher)
        {
            this.logger = logger;
            this.externalKycVerifier = externalKycVerifier;
            this.userRepository = userRepository;
            this.identityService = identityService;
            this.eventPublisher = eventPublisher;
        }

        public async Task Handle(KycSubmittedDomainEvent notification, CancellationToken cancellationToken)
        {
            var kycRequest = new KycVerificationRequest()
            {
                FirstName = notification.FirstName,
                LastName = notification.LastName,
                NID = notification.NID,
            };
            this.logger.Log(LogLevel.Information, $"Value: {notification.FirstName}, {notification.LastName} {notification.NID}");
            var kycVerificationResult = await externalKycVerifier.Verify(kycRequest, notification.User.Country.Name);
            this.logger.Log(LogLevel.Information, $"Value:{notification.NID}, {notification.FirstName}, {notification.LastName}, {kycVerificationResult} ");

            var user = await this.userRepository.Get(notification.User.Id);
            user.UpdateKycStatus((short)kycVerificationResult);
            user.SetVerifiedStatus((short)kycVerificationResult);
            userRepository.Update(user);

            await this.userRepository.UnitOfWork.SaveEntitiesAsync();

            if (user.IsKycVerified)
            {
                eventPublisher.PublishIntegrationEvent(new KycApprovedEvent()
                {
                    UserId = user.Id
                });
            }
        }
    }
}
