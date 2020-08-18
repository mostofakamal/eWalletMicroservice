using System;

namespace Transaction.Domain.Exceptions
{
    public class TransactionDomainException : Exception
    {
        public TransactionDomainException()
        { }

        public TransactionDomainException(string message)
            : base(message)
        { }

        public TransactionDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
