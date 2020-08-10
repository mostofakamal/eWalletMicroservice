using Core.Lib.IntegrationEvents;
using MassTransit;
using System.Threading.Tasks;

namespace Kyc.API.Application.IntegrationEvents
{
    public class UserCreatedIntegratedEventConsumer : IConsumer<UserCreatedIntegrationEvent>
    {
        public UserCreatedIntegratedEventConsumer()
        {

        }
        public Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
        {
            throw new System.NotImplementedException();
        }
    }
}
