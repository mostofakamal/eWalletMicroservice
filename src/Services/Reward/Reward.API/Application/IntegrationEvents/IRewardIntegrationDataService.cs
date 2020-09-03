using Core.Lib.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reward.API.Application.IntegrationEvents
{
    public interface IRewardIntegrationDataService
    {
        Task Publish(Guid transactionId);
        Task AddAndSaveAsync(IntegrationData data);
        Task AddAndSaveAsync(IList<IntegrationData> integrationDataItems);
    }
}
