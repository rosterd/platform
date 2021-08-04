using System;

namespace Rosterd.Domain.Exceptions
{
    public class RoleDoesNotExistException : Exception
    {
        public RoleDoesNotExistException()
        {
        }

        public RoleDoesNotExistException(string message)
            : base(message)
        {
        }

        public RoleDoesNotExistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
