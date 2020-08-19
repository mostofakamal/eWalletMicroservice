using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Transaction.API.Application.Commands
{
    public class CreateTransactionCommand: IRequest<TransactionResponse>
    {
        public decimal Amount { get; set; }

        public Guid SenderUserGuid { get; set; }

        public Guid ReceiverUserGuid { get; set; }

        public int TransactionType { get; set; }

    }
}
