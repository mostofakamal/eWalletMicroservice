using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Transaction.Domain.AggregateModel;
using Transaction.Domain.Services;

namespace Transaction.API.Application.Commands
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand>
    {
        private readonly IUserTransactionService _userTransactionService;

        public CreateTransactionCommandHandler(IUserTransactionService userTransactionService)
        {
            _userTransactionService = userTransactionService;
        }

        public async Task<Unit> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            await _userTransactionService.DoTransaction(request.Amount, request.SenderUserGuid,
                request.ReceiverUserGuid, TransactionType.From(request.TransactionType));
            return Unit.Value;
        }
    }
}