namespace Transaction.Domain.Exceptions
{
    public class SenderReceiverCountryMismatchTransactionDomainException : TransactionDomainException
    {
        public SenderReceiverCountryMismatchTransactionDomainException(string message) : base(message)
        {

        }
    }
}