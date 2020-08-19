using Kyc.API.Application.Commands;
using Kyc.Domain.AggregateModel;
using Kyc.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.API.Application.Services
{
    public class KycVerificationService : IKycVerificationService
    {
        private readonly IUserRepository userRepository;
        private readonly IExternalKycVerifier externalKycVerifier;

        public KycVerificationService(IUserRepository userRepository, IExternalKycVerifier externalKycVerifier)
        {
            this.userRepository = userRepository;
            this.externalKycVerifier = externalKycVerifier;
        }

        public async Task<KycStatuses> SumitKycAsync(SubmitKycCommand request, Guid userId)
        {
            var currentUser = await userRepository.GetAsync(userId);
            if (currentUser.IsKycVerified)
            {
                throw new UserAlreadyVerifiedException("You are already verified.");
            }

            var verifiedUserWithSameNid = await userRepository.GetAsync(request.NID);
            if (verifiedUserWithSameNid != null)
            {
                throw new NidIsAlreadyInUseException("Please try with different NID");
            }

            var kycRequest = new KycVerificationRequest()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                NID = request.NID,
            };

            var kycVerificationResult = await externalKycVerifier.Verify(kycRequest, currentUser.Country.Name);

            var kycInformation = new KycInformation(userId, request.NID, request.FirstName, request.LastName, kycVerificationResult);
            currentUser.AddKyc(kycInformation);
            currentUser.SetVerifiedStatus((short)kycVerificationResult);
            userRepository.Update(currentUser);
            await userRepository.UnitOfWork.SaveChangesAsync();

            return kycVerificationResult;
        }
    }
}
