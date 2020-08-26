using System;

namespace Core.Lib.IntegrationEvents
{
    public interface ITransactionIntegrationMessage
    {
        Guid SenderUserGuid { get; set; }
        Guid ReceiverUserGuid { get; set; }
        decimal Amount { get; set; }
        int TransactionType { get; set; }
    }
}