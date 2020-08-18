using Core.Lib.IntegrationEvents;
using MassTransit;
using System;

namespace Kyc.API.Application.IntegrationEvents
{
    public class KycApprovedEventPublisher : IIntegrationEventPublisher<KycApprovedEvent>
    {
        private readonly IPublishEndpoint _endpoint;

        public KycApprovedEventPublisher(IPublishEndpoint endpoint)
        {
            this._endpoint = endpoint;
        }

        public void PublishIntegrationEvent(KycApprovedEvent kycApprovedEvent)
        {
            this._endpoint.Publish<IKycApprovedEvent>(kycApprovedEvent);
        }
    }
}
