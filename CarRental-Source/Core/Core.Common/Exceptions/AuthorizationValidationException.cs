using System;

namespace Core.Common.Exceptions
{
    public class AuthorizationValidationException : ApplicationException
    {
        public AuthorizationValidationException(string message)
            : base(message) { }

        public AuthorizationValidationException(string message, Exception ex)
            : base(message, ex) { }
    }
}
