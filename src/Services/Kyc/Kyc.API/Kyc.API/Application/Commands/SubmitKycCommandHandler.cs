using Kyc.Domain.AggregateModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;

namespace Kyc.API.Application.Commands
{
    public class SubmitKycCommandHandler : IRequestHandler<SubmitKycCommand, bool>
    {
        private readonly IKycRepository kycRepository;
        private readonly IIdentityService identityService;

        public SubmitKycCommandHandler(IKycRepository kycRepository,IIdentityService identityService)
        {
            this.kycRepository = kycRepository;
            this.identityService = identityService;
        }
        public async Task<bool> Handle(SubmitKycCommand request, CancellationToken cancellationToken)
        {
            var userId = identityService.GetUserIdentity();
            var kycInformation = new KycInformation(Guid.Parse(userId), request.NID, request.FirstName, request.LastName, KycStatuses.Pending);
            await this.kycRepository.Add(kycInformation);
            await this.kycRepository.UnitOfWork.SaveEntitiesAsync();
            return true;
        }
    }
}
