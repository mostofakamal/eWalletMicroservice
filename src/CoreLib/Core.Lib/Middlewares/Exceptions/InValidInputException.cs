using System;

namespace Core.Lib.Middlewares.Exceptions
{
    public class InValidInputException : Exception
    {
        public InValidInputException(string message) : base(message)
        {

        }
    }
}