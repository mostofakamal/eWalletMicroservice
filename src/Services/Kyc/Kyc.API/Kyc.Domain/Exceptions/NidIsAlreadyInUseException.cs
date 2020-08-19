using System;
using System.Collections.Generic;
using System.Text;

namespace Kyc.Domain.Exceptions
{
    public class NidIsAlreadyInUseException: KycDomainException
    {
        public NidIsAlreadyInUseException(string message): base(message) 
        {

        }
    }
}
