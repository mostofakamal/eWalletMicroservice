using Core.Lib.IntegrationEvents;
using MassTransit;
using System;
using System.Threading.Tasks;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;

namespace Transaction.API.Application.IntegrationEvents
{
    public class TransactionIntegrationEventConsumer : IConsumer<TransactionIntegrationMessage>
    {
        private readonly IUserTransactionService userTransactionService;

        public TransactionIntegrationEventConsumer(IUserTransactionService userTransactionService)
        {
            this.userTransactionService = userTransactionService;
        }
        public async Task Consume(ConsumeContext<TransactionIntegrationMessage> context)
        {
            var transactionEvent = context.Message;
            var transactionType = TransactionType.FromValue<TransactionType>(transactionEvent.TransactionType);
            await userTransactionService.DoTransaction(transactionEvent.Amount,
                transactionEvent.SenderUserGuid,
                transactionEvent.ReceiverUserGuid,
                transactionType);
        }
    }
}
