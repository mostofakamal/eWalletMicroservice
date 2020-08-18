using Core.Lib.IdentityServer;
using Kyc.Domain.AggregateModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.API.Application.Commands
{
    public class SubmitKycCommandHandler : IRequestHandler<SubmitKycCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IIdentityService identityService;

        public SubmitKycCommandHandler(IUserRepository kycRepository, IIdentityService identityService)
        {
            this.userRepository = kycRepository;
            this.identityService = identityService;
        }
        public async Task<bool> Handle(SubmitKycCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(identityService.GetUserIdentity());
            var user = await userRepository.Get(userId);
            var kycInformation = new KycInformation(userId, request.NID, request.FirstName, request.LastName, KycStatuses.Pending);
            user.AddKyc(kycInformation);
            return await this.userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
