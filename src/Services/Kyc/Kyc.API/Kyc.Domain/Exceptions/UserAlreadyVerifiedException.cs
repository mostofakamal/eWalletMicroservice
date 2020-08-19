using System;
using System.Collections.Generic;
using System.Text;

namespace Kyc.Domain.Exceptions
{
    public class UserAlreadyVerifiedException: KycDomainException
    {
        public UserAlreadyVerifiedException(string message): base(message)
        {
        }
    }
}
