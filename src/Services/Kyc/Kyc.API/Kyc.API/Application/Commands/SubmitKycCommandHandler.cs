using Kyc.Domain.AggregateModel;
using MediatR;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;
using Kyc.API.Application.Services;
using Kyc.API.Application.IntegrationEvents;
using Core.Lib.IntegrationEvents;

namespace Kyc.API.Application.Commands
{
    public class SubmitKycCommandHandler : IRequestHandler<SubmitKycCommand, SumitKycResponse>
    {
        private readonly IIdentityService identityService;
        private readonly IKycVerificationService kycVerificationService;
        private readonly KycApprovedEventPublisher eventPublisher;
        public SubmitKycCommandHandler(
            IIdentityService identityService,
            IKycVerificationService kycVerificationService,
            KycApprovedEventPublisher eventPublisher)
        {
            this.identityService = identityService;
            this.kycVerificationService = kycVerificationService;
            this.eventPublisher = eventPublisher;
        }

        public async Task<SumitKycResponse> Handle(SubmitKycCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(identityService.GetUserIdentity());
            var kyStatus = await this.kycVerificationService.SumitKycAsync(request, userId);

            if (kyStatus == KycStatuses.Approved)
            {
                eventPublisher.PublishIntegrationEvent(new KycApprovedEvent
                {
                    UserId = userId
                });
            }

            return new SumitKycResponse() { KycStatus = kyStatus.ToString() };
        }
    }
}
