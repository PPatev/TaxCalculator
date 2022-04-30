using System;

namespace TaxCalculator.Infrastructure.Exceptions
{
    public class DuplicateRequestKeyException : Exception
    {
        private readonly string _message;

        public DuplicateRequestKeyException(string message)
        {
            _message = message;
        }

        public override string Message => _message;
    }
}
