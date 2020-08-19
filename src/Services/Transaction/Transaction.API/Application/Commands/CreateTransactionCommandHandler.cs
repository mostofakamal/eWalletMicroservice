using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;

namespace Transaction.API.Application.Commands
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand,TransactionResponse>
    {
        private readonly IUserTransactionService _userTransactionService;
        private readonly ILogger<CreateTransactionCommandHandler> _logger;

        public CreateTransactionCommandHandler(IUserTransactionService userTransactionService, ILogger<CreateTransactionCommandHandler> logger)
        {
            _userTransactionService = userTransactionService;
            _logger = logger;
        }

        public async Task<TransactionResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Doing transaction of Type {TransactionType.From(request.TransactionType).Name} of amount: {request.Amount} from {request.SenderUserGuid} to {request.ReceiverUserGuid}");
            var transactionId = await _userTransactionService.DoTransaction(request.Amount, request.SenderUserGuid,
                request.ReceiverUserGuid, TransactionType.From(request.TransactionType));
            return new TransactionResponse
            {
                TransactionId = transactionId
            };
        }
    }
}