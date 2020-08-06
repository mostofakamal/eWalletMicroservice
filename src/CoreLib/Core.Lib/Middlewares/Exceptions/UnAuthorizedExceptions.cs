using System;

namespace Core.Lib.Middlewares.Exceptions
{
    public class UnAuthorizedExceptions : Exception
    {
        public UnAuthorizedExceptions(string message) : base(message)
        {

        }
    }
}