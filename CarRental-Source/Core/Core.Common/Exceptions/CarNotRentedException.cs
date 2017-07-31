using System;

namespace Core.Common.Exceptions
{
    public class CarNotRentedException : ApplicationException
    {
        public CarNotRentedException(string message)
            : base(message) { }

        public CarNotRentedException(string message, Exception ex)
            : base(message, ex) { }
    }
}
