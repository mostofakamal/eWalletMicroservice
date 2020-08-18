namespace Transaction.Domain.Exceptions
{
    public class TransactionInEligibleDomainException : TransactionDomainException
    {
        public TransactionInEligibleDomainException(string message) : base(message)
        {

        }
    }
}