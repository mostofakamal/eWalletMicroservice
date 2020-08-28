using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;

namespace Transaction.API.Application.IntegrationEvents
{
    public interface ITransactionIntegrationDataService
    {
        Task Publish(Guid transactionId);
        Task AddAndSaveAsync(IntegrationData data);
        Task AddAndSaveAsync(IList<IntegrationData> integrationDataItems);
    }
}