using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Transaction.Domain.Services;

namespace Transaction.API.Application.Commands
{
    public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand, TransactionResponse>
    {
        private readonly IUserTransactionService _userTransactionService;
        private readonly IIdentityService _identityService;
        private readonly ILogger<TransferMoneyCommandHandler> _logger;

        public TransferMoneyCommandHandler(IUserTransactionService userTransactionService,
            IIdentityService identityService, ILogger<TransferMoneyCommandHandler> logger)
        {
            _userTransactionService = userTransactionService;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<TransactionResponse> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {

            var senderUserGuid = Guid.Parse(_identityService.GetUserIdentity());
            _logger.LogInformation($"Doing money transfer of amount: {request.Amount} from {senderUserGuid} to {request.ReceiverPhoneNumber}");
            var transactionId = await _userTransactionService.TransferMoney(request.Amount, senderUserGuid, request.ReceiverPhoneNumber);
            return new TransactionResponse { TransactionId = transactionId };
        }
    }
}