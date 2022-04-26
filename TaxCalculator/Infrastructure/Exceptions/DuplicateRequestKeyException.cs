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

        public string Message => _message;
    }
}
