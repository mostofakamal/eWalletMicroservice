using System;
using MediatR;

namespace Transaction.API.Application.Commands
{
    public class TransferMoneyCommand : IRequest
    {
        public decimal Amount { get; set; }

        public Guid ReceiverUserGuid { get; set; }

    }
}