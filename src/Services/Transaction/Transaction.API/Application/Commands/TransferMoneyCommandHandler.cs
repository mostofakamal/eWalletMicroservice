using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Lib.Services;
using MediatR;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;

namespace Transaction.API.Application.Commands
{
    public class TransferMoneyCommandHandler : IRequestHandler<TransferMoneyCommand>
    {
        private readonly IUserTransactionService _userTransactionService;
        private readonly IIdentityService _identityService;

        public TransferMoneyCommandHandler(IUserTransactionService userTransactionService, IIdentityService identityService)
        {
            _userTransactionService = userTransactionService;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {
            var senderUserGuid = Guid.Parse(_identityService.GetUserIdentity());
            await _userTransactionService.DoTransaction(request.Amount, senderUserGuid, request.ReceiverUserGuid,
                TransactionType.Transfer);
            return Unit.Value;
        }
    }
}