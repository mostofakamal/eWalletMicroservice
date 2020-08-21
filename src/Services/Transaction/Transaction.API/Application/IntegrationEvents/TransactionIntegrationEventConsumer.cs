using Core.Lib.IntegrationEvents;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;

namespace Transaction.API.Application.IntegrationEvents
{
    public class TransactionIntegrationEventConsumer : IConsumer<ITransactionIntegrationEvent>
    {
        private readonly IUserTransactionService userTransactionService;

        public TransactionIntegrationEventConsumer(IUserTransactionService userTransactionService)
        {
            this.userTransactionService = userTransactionService;
        }
        public async Task Consume(ConsumeContext<ITransactionIntegrationEvent> context)
        {
            var transactionEvent = context.Message;
            var transactionType = TransactionType.FromValue<TransactionType>(transactionEvent.TransactionType);
            
            await this.userTransactionService.DoTransaction(transactionEvent.Amount,
                transactionEvent.SenderUserGuid,
                transactionEvent.ReceiverUserGuid,
                transactionType);
        }
    }
}
