﻿using Core.Lib.IntegrationEvents;
using IntegrationDataLog;
using IntegrationDataLog.Services;
using Kyc.Insfrastructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Kyc.API.Application.IntegrationEvents
{
    public class KycIntegrationDataService : IKycIntegrationDataService
    {
        private readonly Func<DbConnection, ILogger<IIntegrationDataLogService>, IIntegrationDataLogService> _integrationDataLogServiceFactory;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpoint _sendEndpoint;
        private readonly UserContext _transactionContext;
        private readonly IIntegrationDataLogService _dataLogService;
        private readonly ILogger<IIntegrationDataLogService> _logger;

        public KycIntegrationDataService(
            UserContext transactionContext,
            Func<DbConnection, ILogger<IIntegrationDataLogService>, IIntegrationDataLogService> integrationEventLogServiceFactory,
            ILogger<IIntegrationDataLogService> logger, IPublishEndpoint endpoint,
            ISendEndpointProvider sendEndpointProvider, IBusControl busControl)
        {
            _transactionContext = transactionContext ?? throw new ArgumentNullException(nameof(transactionContext));
            _integrationDataLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _publishEndpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _sendEndpoint = sendEndpointProvider.GetSendEndpoint(busControl.Address).Result;
            _dataLogService = _integrationDataLogServiceFactory(transactionContext.Database.GetDbConnection(), logger);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Publish(Guid transactionId)
        {
            var pendingLogData = await _dataLogService.RetrieveIntegrationDataLogsPendingToPublishAsync(transactionId);

            foreach (var logData in pendingLogData)
            {
                _logger.LogInformation("----- Publishing integration data: {IntegrationEventId} from TransactionService - ({@IntegrationData})", logData.IntegrationDataId, logData.IntegrationData);

                try
                {
                    await _dataLogService.MarkDataAsInProgressAsync(logData.IntegrationDataId);
                    var messageType = typeof(IntegrationEvent).Assembly.GetType(logData.DataTypeName);
                    _logger.LogInformation($"Message type name: {logData.DataTypeName} and type: {messageType} and DataType: {logData.IntegrationDataType}");
                    await SendOrPublishDataToQueue(logData, messageType);
                    await _dataLogService.MarkDataAsPublishedAsync(logData.IntegrationDataId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration data: {IntegrationEventId} from TransactionService", logData.IntegrationDataId);

                    await _dataLogService.MarkDataAsFailedAsync(logData.IntegrationDataId);
                }
            }
        }

        private async Task SendOrPublishDataToQueue(IntegrationDataLogEntry logData, Type messageType)
        {
            if (logData.IntegrationDataType == IntegrationDataType.Event)
            {
                await _publishEndpoint.Publish(logData.IntegrationData, messageType);
            }
            else
            {
                await _sendEndpoint.Send(logData.IntegrationData, messageType);
            }
        }

        public async Task AddAndSaveAsync(IntegrationData data)
        {
            _logger.LogInformation("----- Enqueuing integration data {IntegrationDataId} to repository ({@IntegrationData})", data.Id, data);

            await _dataLogService.SaveIntegrationDataAsync(data, _transactionContext.GetCurrentTransaction());
        }
    }
}
