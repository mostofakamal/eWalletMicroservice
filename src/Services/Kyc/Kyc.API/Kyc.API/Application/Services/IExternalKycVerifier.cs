using Kyc.Domain.AggregateModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.API.Application.Services
{
    public interface IExternalKycVerifier
    {
        Task<KycStatuses> Verify(KycVerificationRequest kycRequest, string countryName);
    }
}
