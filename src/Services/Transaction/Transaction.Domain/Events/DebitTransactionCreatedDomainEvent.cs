﻿using System;
using MediatR;
using Transaction.Domain.AggregateModel;

namespace Transaction.Domain.Events
{
    public class DebitTransactionCreatedDomainEvent: INotification
    {
        public Guid TransactionGuid { get; }

        public Guid SenderUserGuid { get; }

        public Guid ReceiverUserGuid { get; }

        public decimal Amount { get; }

        public TransactionType TransactionType { get; }

        public Guid CorrelationId { get; }

        public DebitTransactionCreatedDomainEvent(decimal amount,Guid senderUserGuid,Guid receiverUserGuid,TransactionType type,Guid transactionId,Guid correlationId)
        {
            Amount = amount;
            SenderUserGuid = senderUserGuid;
            ReceiverUserGuid = receiverUserGuid;
            TransactionType = type;
            TransactionGuid = transactionId;
            CorrelationId = correlationId;
        }

    }
}
