using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using Core.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Reward.API.Application.Commands;
using Reward.Domain.AggregateModel;

namespace Reward.API.Application.IntegrationEvents
{
    public class KycApprovedIntegrationEventInRewardConsumer : IConsumer<KycApprovedIntegrationEvent>
    {
        private readonly ILogger<KycApprovedIntegrationEvent> _logger;
        private readonly IMediator mediator;

        public KycApprovedIntegrationEventInRewardConsumer(
             ILogger<KycApprovedIntegrationEvent> logger,
             IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        public async Task Consume(ConsumeContext<KycApprovedIntegrationEvent> context)
        {
            var kycApprovedEvent = context.Message;
            _logger.LogInformation($"Consuming {nameof(KycApprovedIntegrationEvent)} inside reward service..");
            await this.mediator.Send<bool>(new ProcessRewardForKyc() { UserId = kycApprovedEvent.UserId });
        }
    }
}