using Core.Lib.IntegrationEvents;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Kyc.API.Application.IntegrationEvents
{
    public class TransactionIntegrationMessageConsumer : IConsumer<ITransactionIntegrationMessage>
    {
        public async Task Consume(ConsumeContext<ITransactionIntegrationMessage> context)
        {
            Console.WriteLine("Message recieved: " + context.Message.Amount);
        }
    }
}
