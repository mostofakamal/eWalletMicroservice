using Core.Lib.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.API.Application.IntegrationEvents
{
    public interface IKycIntegrationDataService
    {
        Task Publish(Guid transactionId);
        Task AddAndSaveAsync(IntegrationData data);
    }
}
