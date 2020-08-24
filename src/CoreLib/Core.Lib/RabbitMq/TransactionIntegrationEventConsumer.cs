using Core.Lib.IntegrationEvents;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Core.Lib.RabbitMq
{
    public class TransactionIntegrationEventConsumer1 : IConsumer<TransactionIntegrationMessage>
    {
        public async Task Consume(ConsumeContext<TransactionIntegrationMessage> context)
        {
            Console.WriteLine("Consumed from consumer");
        }
    }
}
