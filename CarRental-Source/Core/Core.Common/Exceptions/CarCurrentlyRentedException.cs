using System;

namespace Core.Common.Exceptions
{
    public class CarCurrentlyRentedException : ApplicationException
    {
        public CarCurrentlyRentedException(string message)
            : base(message) { }

        public CarCurrentlyRentedException(string message, Exception ex)
            : base(message, ex) { }
    }
}
