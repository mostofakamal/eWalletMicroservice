using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Reward.Domain.AggregateModel;

namespace Reward.API.Application.IntegrationEvents
{
    public class UserCreatedIntegrationEventInRewardConsumer : IConsumer<IUserCreatedIntegrationEvent>
    {
        private readonly ILogger<UserCreatedIntegrationEventInRewardConsumer> _logger;
        private readonly IUserRepository _userRepository;

        public UserCreatedIntegrationEventInRewardConsumer(ILogger<UserCreatedIntegrationEventInRewardConsumer> logger, IUserRepository userRepository)
        {
            this._logger = logger;
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<IUserCreatedIntegrationEvent> context)
        {
            var userRegistrationEvent = context.Message;
            _logger.LogInformation($"Consuming {nameof(UserCreatedIntegrationEvent)} inside reward service..");
            var user = new User(userRegistrationEvent.UserGuid, userRegistrationEvent.CountryId, userRegistrationEvent.PhoneNumber);
            _userRepository.Add(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
