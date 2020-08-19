using Core.Lib.Services;
using Kyc.Domain.AggregateModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kyc.API.Application.Queries
{
    public class KycHistoryQueryHandler : IRequestHandler<GetKycHistoryQuery, KycHistoryResponse>
    {
        private readonly IIdentityService identityService;
        private readonly IUserRepository userRepository;

        public KycHistoryQueryHandler(IIdentityService identityService, IUserRepository userRepository)
        {
            this.identityService = identityService;
            this.userRepository = userRepository;
        }

        public async Task<KycHistoryResponse> Handle(GetKycHistoryQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(identityService.GetUserIdentity());
            var user = await this.userRepository.GetAsync(currentUserId);
            var kycHistories = user.KycInformations.Select(k =>
            new KycHistory()
            {
                CreatedTime = k.CreatedTime,
                FirstName = k.FirstName,
                KycStatus = Enum.GetName(typeof(KycStatuses), k.KycStatusId),
                LastName = k.LastName,
                NID = k.NID
            });

            return new KycHistoryResponse() { KycHistories = new List<KycHistory>(kycHistories) };
        }
    }
}
