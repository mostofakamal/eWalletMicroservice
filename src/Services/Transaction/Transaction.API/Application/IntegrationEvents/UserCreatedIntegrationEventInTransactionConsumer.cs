using System.Threading.Tasks;
using Core.Lib.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using Transaction.Domain.AggregateModel;

namespace Transaction.API.Application.IntegrationEvents
{
    public class UserCreatedIntegrationEventInTransactionConsumer : IConsumer<IUserCreatedIntegrationEvent>
    {
        private readonly ILogger<UserCreatedIntegrationEventInTransactionConsumer> _logger;
        private readonly IUserRepository _userRepository;

        public UserCreatedIntegrationEventInTransactionConsumer(ILogger<UserCreatedIntegrationEventInTransactionConsumer> logger, IUserRepository userRepository)
        {
            this._logger = logger;
            _userRepository = userRepository;
        }
        public async Task Consume(ConsumeContext<IUserCreatedIntegrationEvent> context)
        {
            var userRegistrationEvent = context.Message;
            _logger.LogInformation($"Consuming {nameof(UserCreatedIntegrationEvent)} inside transaction service..");
            var user = new User(userRegistrationEvent.UserGuid, userRegistrationEvent.CountryId, userRegistrationEvent.PhoneNumber);
            _userRepository.Add(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
