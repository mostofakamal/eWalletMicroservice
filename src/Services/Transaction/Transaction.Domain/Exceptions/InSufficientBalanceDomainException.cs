namespace Transaction.Domain.Exceptions
{
    public class InSufficientBalanceDomainException : TransactionDomainException
    {
        public InSufficientBalanceDomainException(string message): base(message)
        {
            
        }
    }
}