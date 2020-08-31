using Core.Lib.IntegrationEvents;
using MassTransit;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;

namespace Transaction.API.Application.IntegrationEvents
{
    public class TransactionIntegrationEventConsumer : IConsumer<TransactionIntegrationMessage>
    {
        private readonly IUserTransactionService userTransactionService;
        private readonly ILogger<TransactionIntegrationEventConsumer> _logger;

        public TransactionIntegrationEventConsumer(IUserTransactionService userTransactionService, ILogger<TransactionIntegrationEventConsumer> logger)
        {
            this.userTransactionService = userTransactionService;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<TransactionIntegrationMessage> context)
        {
           
            var transactionEvent = context.Message;
            _logger.LogInformation($"Consuming TransactionIntegrationMessage with amount : {transactionEvent.Amount} Type: {transactionEvent.TransactionType} senderUserGuid: {transactionEvent.SenderUserGuid} receiverUserGuid: {transactionEvent.ReceiverUserGuid}");
            var transactionType = TransactionType.From(transactionEvent.TransactionType);
            await userTransactionService.DoTransaction(transactionEvent.Amount,
                transactionEvent.SenderUserGuid,
                transactionEvent.ReceiverUserGuid,
                transactionType);
        }
    }
}
