﻿using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Reward.Domain.AggregateModel;

namespace Reward.API.Application.IntegrationEvents
{
    public class KycApprovedIntegrationEventConsumer : IConsumer<KycApprovedIntegrationEvent>
    {
        private readonly ILogger<KycApprovedIntegrationEvent> _logger;
        private readonly IUserRepository _userRepository;

        public KycApprovedIntegrationEventConsumer(ILogger<KycApprovedIntegrationEvent> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<KycApprovedIntegrationEvent> context)
        {
            var kycApprovedEvent = context.Message;
            _logger.LogInformation($"Consuming {nameof(KycApprovedIntegrationEvent)} inside reward service..");
            var user = await _userRepository.GetAsync(kycApprovedEvent.UserId);
            if (user != null)
            {
                ///TODO:Do Transaction
                await _userRepository.UnitOfWork.SaveEntitiesAsync();
            }
            else
            {
                _logger.LogWarning($"User with Id:  {kycApprovedEvent.UserId} does not exist in reward service");
            }
           
        }
    }
}