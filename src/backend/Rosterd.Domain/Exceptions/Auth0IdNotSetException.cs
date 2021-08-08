using System;

namespace Rosterd.Domain.Exceptions
{
    public class Auth0IdNotSetException : Exception
    {
        public Auth0IdNotSetException()
        {
        }

        public Auth0IdNotSetException(string message)
            : base(message)
        {
        }

        public Auth0IdNotSetException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
