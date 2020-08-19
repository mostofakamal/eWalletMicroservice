using Kyc.API.Application.Commands;
using Kyc.Domain.AggregateModel;
using System;
using System.Threading.Tasks;

namespace Kyc.API.Application.Services
{
    public interface IKycVerificationService
    {
        Task<KycStatuses> SumitKycAsync(SubmitKycCommand request, Guid userId);
    }
}