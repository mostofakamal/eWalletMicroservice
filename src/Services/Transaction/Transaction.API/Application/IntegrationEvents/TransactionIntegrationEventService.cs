using System;
using System.Data.Common;
using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using IntegrationEventLog.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransactionContext = Transaction.Infrastructure.TransactionContext;

namespace Transaction.API.Application.IntegrationEvents
{
    public class TransactionIntegrationEventService : ITransactionIntegrationEventService
    {
        private readonly Func<DbConnection, ILogger<IIntegrationEventLogService>, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IPublishEndpoint _endpoint;
        private readonly TransactionContext _transacitonContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<IIntegrationEventLogService> _logger;

        public TransactionIntegrationEventService(
            TransactionContext transactionContext,
            Func<DbConnection, ILogger<IIntegrationEventLogService>, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<IIntegrationEventLogService> logger,IPublishEndpoint endpoint)
        {
            _transacitonContext = transactionContext ?? throw new ArgumentNullException(nameof(transactionContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _eventLogService = _integrationEventLogServiceFactory(transactionContext.Database.GetDbConnection(),logger);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from TransactionService - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    //await _endpoint.Publish(logEvt.IntegrationEvent);
                    var messageType = typeof(IntegrationEvent).Assembly.GetType(logEvt.EventTypeName);
                    _logger.LogInformation($"Message type name: {logEvt.EventTypeName} and type: {messageType}");
                    await _endpoint.Publish(logEvt.IntegrationEvent, messageType);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from TransactionService", logEvt.EventId);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await _eventLogService.SaveEventAsync(evt, _transacitonContext.GetCurrentTransaction());
        }
    }
}