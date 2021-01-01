using System;

namespace Rosterd.Domain.Exceptions
{
    public class ResourceDoesNotBelongToTenantException : Exception
    {
        public ResourceDoesNotBelongToTenantException()
        {
        }

        public ResourceDoesNotBelongToTenantException(string message)
            : base(message)
        {
        }

        public ResourceDoesNotBelongToTenantException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
