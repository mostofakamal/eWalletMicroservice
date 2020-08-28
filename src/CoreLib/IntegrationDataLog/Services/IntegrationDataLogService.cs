using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace IntegrationDataLog.Services
{
    public class IntegrationDataLogService : IIntegrationDataLogService
    {
        private readonly IntegrationDataLogContext _integrationDataLogContext;
        private readonly DbConnection _dbConnection;
        private readonly List<Type> _eventTypes;
        private readonly ILogger<IIntegrationDataLogService> _logger;

        public IntegrationDataLogService(DbConnection dbConnection, ILogger<IIntegrationDataLogService> logger)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _logger = logger;
            _integrationDataLogContext = new IntegrationDataLogContext(
                new DbContextOptionsBuilder<IntegrationDataLogContext>()
                    .UseSqlServer(_dbConnection)
                    .Options);

            _eventTypes = Assembly.Load(typeof(IntegrationData).Assembly.FullName)
                .GetTypes()
                //.Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationDataLogEntry>> RetrieveIntegrationDataLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var result = await _integrationDataLogContext.IntegrationDataLogs
                .Where(e => e.TransactionId == tid && e.State == IntegrationDataStateEnum.NotPublished).ToListAsync();

            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)));
            }

            return new List<IntegrationDataLogEntry>();
        }

        public Task SaveIntegrationDataAsync(IntegrationData data, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventLogEntry = new IntegrationDataLogEntry(data, transaction.TransactionId);

            _integrationDataLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _integrationDataLogContext.IntegrationDataLogs.Add(eventLogEntry);

            return _integrationDataLogContext.SaveChangesAsync();
        }

        public Task SaveIntegrationDataAsync(IList<IntegrationData> dataItems, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            IList<IntegrationDataLogEntry> eventLogEntries = new List<IntegrationDataLogEntry>();
            foreach (var data in dataItems)
            {
                eventLogEntries.Add(new IntegrationDataLogEntry(data, transaction.TransactionId));
            }
       

            _integrationDataLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _integrationDataLogContext.IntegrationDataLogs.AddRange(eventLogEntries);

            return _integrationDataLogContext.SaveChangesAsync();
        }


        public Task MarkDataAsPublishedAsync(Guid eventId)
        {
            return UpdateIntegrationDataStatus(eventId, IntegrationDataStateEnum.Published);
        }

        public Task MarkDataAsInProgressAsync(Guid eventId)
        {
            return UpdateIntegrationDataStatus(eventId, IntegrationDataStateEnum.InProgress);
        }

        public Task MarkDataAsFailedAsync(Guid eventId)
        {
            return UpdateIntegrationDataStatus(eventId, IntegrationDataStateEnum.PublishedFailed);
        }

        private Task UpdateIntegrationDataStatus(Guid eventId, IntegrationDataStateEnum status)
        {
            var integrationDataLogEntry = _integrationDataLogContext.IntegrationDataLogs.Single(ie => ie.IntegrationDataId == eventId);
            integrationDataLogEntry.State = status;

            if (status == IntegrationDataStateEnum.InProgress)
                integrationDataLogEntry.TimesSent++;

            _integrationDataLogContext.IntegrationDataLogs.Update(integrationDataLogEntry);

            return _integrationDataLogContext.SaveChangesAsync();
        }
    }
}