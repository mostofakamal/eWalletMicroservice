using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.API.Application.Queries
{
    public class GetKycHistoryQuery : IRequest<KycHistoryResponse>
    {
    }
}
