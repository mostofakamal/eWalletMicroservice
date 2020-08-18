using Core.Lib.IdentityServer;
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

namespace Kyc.API.Application.DomainEventHandlers
{
    public class KycSubmittedDomainEventHandler : INotificationHandler<KycSubmittedDomainEvent>
    {
        private readonly ILogger<KycSubmittedDomainEventHandler> logger;
        private readonly IExternalKycVerifier externalKycVerifier;
        private readonly IUserRepository userRepository;
        private readonly IPublishEndpoint endpoint;
        private readonly IIdentityService identityService;

        public KycSubmittedDomainEventHandler(ILogger<KycSubmittedDomainEventHandler> logger,
            IExternalKycVerifier externalKycVerifier,
            IUserRepository userRepository,
            IPublishEndpoint endpoint,
            IIdentityService identityService)
        {
            this.logger = logger;
            this.externalKycVerifier = externalKycVerifier;
            this.userRepository = userRepository;
            this.endpoint = endpoint;
            this.identityService = identityService;
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

            var user = await this.userRepository.Get(notification.UserId);
            user.UpdateKyc(notification.KycId, (short)kycVerificationResult);

            await this.userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
