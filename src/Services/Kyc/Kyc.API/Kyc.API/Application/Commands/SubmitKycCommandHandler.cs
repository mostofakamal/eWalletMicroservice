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
        private readonly IKycRepository kycRepository;

        public SubmitKycCommandHandler(IKycRepository kycRepository)
        {
            this.kycRepository = kycRepository;
        }
        public async Task<bool> Handle(SubmitKycCommand request, CancellationToken cancellationToken)
        {
            var kycInformation = new KycInformation(Guid.Parse("859c1ff2-f5bc-48f1-915b-beb1e7778b76"), request.NID, request.FirstName, request.LastName, KycStatuses.Pending);
            await this.kycRepository.Add(kycInformation);
            await this.kycRepository.UnitOfWork.SaveEntitiesAsync();
            return true;
        }
    }
}
