using System;
using System.Threading;
using Core.Lib.IntegrationEvents;
using MassTransit;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Exceptions;
using Transaction.Domain.Services;
using IMediator = MediatR.IMediator;

namespace Transaction.API.Application.IntegrationEvents
{
    public class ConsumeTransactionMessageCommand : IRequest
    {
        public TransactionIntegrationMessage Message { get; set; }
    }
    public class TransactionIntegrationMessageConsumeHandler : IRequestHandler<ConsumeTransactionMessageCommand>
    {
        private readonly IUserTransactionService _userTransactionService;
        private readonly ILogger<TransactionIntegrationEventConsumer> _logger;
        private readonly ITransactionIntegrationDataService _transactionIntegrationDataService;

        public TransactionIntegrationMessageConsumeHandler(IUserTransactionService userTransactionService, ILogger<TransactionIntegrationEventConsumer> logger, ITransactionIntegrationDataService transactionIntegrationDataService)
        {
            _userTransactionService = userTransactionService;
            _logger = logger;
            _transactionIntegrationDataService = transactionIntegrationDataService;
        }

        public async Task<Unit> Handle(ConsumeTransactionMessageCommand request, CancellationToken cancellationToken)
        {
            var transactionEvent = request.Message;
            try
            {
                _logger.LogInformation(
                    $"Consuming TransactionIntegrationMessage with amount : {transactionEvent.Amount} Type: {transactionEvent.TransactionType} senderUserGuid: {transactionEvent.SenderUserGuid} receiverUserGuid: {transactionEvent.ReceiverUserGuid}");
                var transactionType = TransactionType.From(transactionEvent.TransactionType);
                await _userTransactionService.DoTransaction(transactionEvent.Amount,
                    transactionEvent.SenderUserGuid,
                    transactionEvent.ReceiverUserGuid,
                    transactionType, transactionEvent.CorrelationId);

            }
            catch(Exception ex)
            {
                // TODO: TransactionFailedEvent Integration event with Correlation Id
                var failedReason = "System Failure";

                if (ex is TransactionDomainException)
                {
                    failedReason = ex.Message;
                }
                var transactionFailedException = new TransactionFailedIntegrationEvent(transactionEvent.Amount,
                    transactionEvent.SenderUserGuid, transactionEvent.ReceiverUserGuid,
                    transactionEvent.TransactionType, transactionEvent.CorrelationId,failedReason);
                await _transactionIntegrationDataService.AddAndSaveAsync(transactionFailedException);
            }
            return Unit.Value;
        }
    }

    public class TransactionIntegrationEventConsumer : IConsumer<TransactionIntegrationMessage>
    {
        private readonly IMediator _mediator;

        public TransactionIntegrationEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
           
        }
        public async Task Consume(ConsumeContext<TransactionIntegrationMessage> context)
        {
            var transactionEvent = context.Message;
            await _mediator.Send(new ConsumeTransactionMessageCommand
            {
                Message = transactionEvent
            });
        }
    }
}
