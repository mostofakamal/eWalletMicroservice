﻿using Core.Lib.IntegrationEvents;
using IntegrationDataLog;
using IntegrationDataLog.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reward.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Reward.API.Application.IntegrationEvents
{
    public class RewardIntegrationDataService : IRewardIntegrationDataService
    {
        private readonly Func<DbConnection, ILogger<IIntegrationDataLogService>, IIntegrationDataLogService> _integrationDataLogServiceFactory;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly RewardContext _rewardContext;
        private readonly IIntegrationDataLogService _dataLogService;
        private readonly ILogger<IIntegrationDataLogService> _logger;

        public RewardIntegrationDataService(
            RewardContext rewardContext,
            Func<DbConnection, ILogger<IIntegrationDataLogService>, IIntegrationDataLogService> integrationEventLogServiceFactory,
            ILogger<IIntegrationDataLogService> logger,
            IPublishEndpoint endpoint,
            ISendEndpointProvider sendEndpointProvider)
        {
            _rewardContext = rewardContext ?? throw new ArgumentNullException(nameof(rewardContext));
            _integrationDataLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _publishEndpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
            _sendEndpointProvider = sendEndpointProvider;
            _dataLogService = _integrationDataLogServiceFactory(rewardContext.Database.GetDbConnection(), logger);
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
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:" + logData.EventTypeShortName));
                await sendEndpoint.Send(logData.IntegrationData, messageType);
            }
        }

        public async Task AddAndSaveAsync(IntegrationData data)
        {
            _logger.LogInformation("----- Enqueuing integration data {IntegrationDataId} to repository ({@IntegrationData})", data.Id, data);

            await _dataLogService.SaveIntegrationDataAsync(new List<IntegrationData> { data }, _rewardContext.GetCurrentTransaction());
        }

        public async Task AddAndSaveAsync(IList<IntegrationData> integrationDataItems)
        {
            // _logger.LogInformation("----- Enqueuing integration data {IntegrationDataId} to repository ({@IntegrationData})", data.Id, data);
            await _dataLogService.SaveIntegrationDataAsync(integrationDataItems, _rewardContext.GetCurrentTransaction());

        }
    }
}
