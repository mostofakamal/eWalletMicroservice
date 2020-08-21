using System;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;

namespace Transaction.API.Application.IntegrationEvents
{
    public interface ITransactionIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}