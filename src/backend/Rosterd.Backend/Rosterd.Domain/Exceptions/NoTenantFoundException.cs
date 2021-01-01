using System;

namespace Rosterd.Domain.Exceptions
{
    public class NoTenantFoundException : Exception
    {
        public NoTenantFoundException()
        {
        }

        public NoTenantFoundException(string message)
            : base(message)
        {
        }

        public NoTenantFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
