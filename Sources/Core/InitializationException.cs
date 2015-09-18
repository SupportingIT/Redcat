using System;

namespace Redcat.Core
{
    public class InitializationException : Exception
    {
        public InitializationException()
        { }

        public InitializationException(string message) : base(message)
        { }

        public InitializationException(string message, Exception exception) : base(message, exception)
        { }
    }
}
