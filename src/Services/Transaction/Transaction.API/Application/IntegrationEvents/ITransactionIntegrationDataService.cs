using System;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;

namespace Transaction.API.Application.IntegrationEvents
{
    public interface ITransactionIntegrationDataService
    {
        Task Publish(Guid transactionId);
        Task AddAndSaveAsync(IntegrationData data);
    }
}