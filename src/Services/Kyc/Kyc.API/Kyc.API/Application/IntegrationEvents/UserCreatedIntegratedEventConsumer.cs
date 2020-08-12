using Core.Lib.IntegrationEvents;
using Kyc.Domain.AggregateModel;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Kyc.API.Application.IntegrationEvents
{
    public class UserCreatedIntegratedEventConsumer : IConsumer<IUserCreatedIntegrationEvent>
    {
        private readonly ILogger<UserCreatedIntegratedEventConsumer> logger;
        private readonly IKycRepository kycRepository;

        public UserCreatedIntegratedEventConsumer(ILogger<UserCreatedIntegratedEventConsumer> logger,
            IKycRepository kycRepository)
        {
            this.logger = logger;
            this.kycRepository = kycRepository;
        }
        public async Task Consume(ConsumeContext<IUserCreatedIntegrationEvent> context)
        {
            IUserCreatedIntegrationEvent message = context.Message;
            this.logger.Log(LogLevel.Information, $"user registred event fired on kyc {message.UserGuid}");
            var user = new User(message.UserGuid, false);

            await this.kycRepository.Add(user);
            await this.kycRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
