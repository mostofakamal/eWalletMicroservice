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
        private readonly IUserRepository userRepository;

        public UserCreatedIntegratedEventConsumer(ILogger<UserCreatedIntegratedEventConsumer> logger,
            IUserRepository userRepository)
        {
            this.logger = logger;
            this.userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<IUserCreatedIntegrationEvent> context)
        {
            IUserCreatedIntegrationEvent message = context.Message;
            this.logger.Log(LogLevel.Information, $"user registred event fired on kyc {message.UserGuid}");
            var user = new User(message.UserGuid, false, message.CountryId);

            await this.userRepository.Add(user);
            await this.userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
