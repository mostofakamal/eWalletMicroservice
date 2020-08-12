using Core.Lib.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Kyc.API.Application.IntegrationEvents
{
    public class UserCreatedIntegratedEventConsumer : IConsumer<IUserCreatedIntegrationEvent>
    {
        private readonly ILogger<UserCreatedIntegratedEventConsumer> logger;

        public UserCreatedIntegratedEventConsumer(ILogger<UserCreatedIntegratedEventConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<IUserCreatedIntegrationEvent> context)
        {
            IUserCreatedIntegrationEvent message = context.Message;
            this.logger.Log(LogLevel.Information, $"user registred event fired on kyc {message.UserGuid}");
        }
    }
}
