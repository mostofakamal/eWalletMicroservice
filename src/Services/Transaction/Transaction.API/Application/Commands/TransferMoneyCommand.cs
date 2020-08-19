using System;
using MediatR;

namespace Transaction.API.Application.Commands
{
    public class TransferMoneyCommand : IRequest<TransactionResponse>
    {
        public decimal Amount { get; set; }

        public string ReceiverPhoneNumber { get; set; }

    }
}