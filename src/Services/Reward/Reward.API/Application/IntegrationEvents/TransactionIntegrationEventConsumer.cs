using Core.Lib.IntegrationEvents;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reward.API.Application.IntegrationEvents
{
    public class TransactionIntegrationEventConsumer : IConsumer<TransactionIntegrationMessage>
    {
        public Task Consume(ConsumeContext<TransactionIntegrationMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
