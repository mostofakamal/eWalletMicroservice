using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationDataLog.Services
{
    public interface IIntegrationDataLogService
    {
        Task<IEnumerable<IntegrationDataLogEntry>> RetrieveIntegrationDataLogsPendingToPublishAsync(Guid transactionId);
        Task SaveIntegrationDataAsync(IntegrationData data, IDbContextTransaction transaction);
        Task MarkDataAsPublishedAsync(Guid eventId);
        Task MarkDataAsInProgressAsync(Guid eventId);
        Task MarkDataAsFailedAsync(Guid eventId);
        Task SaveIntegrationDataAsync(IList<IntegrationData> dataItems, IDbContextTransaction transaction);
    }
}
