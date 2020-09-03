using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Reward.API.Application.Commands;

namespace Reward.API.Application.IntegrationEvents
{
    public class KycApprovedIntegrationEventInRewardConsumer : IConsumer<KycApprovedIntegrationEvent>
    {
        private readonly ILogger<KycApprovedIntegrationEvent> _logger;
        private readonly MediatR.IMediator _mediator;

        public KycApprovedIntegrationEventInRewardConsumer(
             ILogger<KycApprovedIntegrationEvent> logger,
             MediatR.IMediator mediator)
        {
            _logger = logger;
            this._mediator = mediator;
        }

        public async Task Consume(ConsumeContext<KycApprovedIntegrationEvent> context)
        {
            var kycApprovedEvent = context.Message;
            _logger.LogInformation($"Consuming {nameof(KycApprovedIntegrationEvent)} inside reward service..");
            await this._mediator.Send<bool>(new ProcessRewardForKyc() { UserId = kycApprovedEvent.UserId });
        }
    }
}