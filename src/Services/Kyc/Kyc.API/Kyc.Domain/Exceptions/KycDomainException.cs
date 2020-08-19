using System;

namespace Kyc.Domain.Exceptions
{
    public class KycDomainException: Exception
    {
        public KycDomainException()
        {

        }

        public KycDomainException(string message) : base(message)
        {

        }

        public KycDomainException(string message, Exception innnerException)
            : base(message, innnerException)
        {

        }
    }
}
