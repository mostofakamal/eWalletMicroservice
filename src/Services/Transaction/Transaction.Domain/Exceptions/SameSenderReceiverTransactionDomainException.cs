namespace Transaction.Domain.Exceptions
{
    public class SameSenderReceiverTransactionDomainException : TransactionDomainException
    {
        public SameSenderReceiverTransactionDomainException(string message) : base(message)
        {

        }
    }

}