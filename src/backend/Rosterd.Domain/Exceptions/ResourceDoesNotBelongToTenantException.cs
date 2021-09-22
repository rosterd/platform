using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class ResourceDoesNotBelongToTenantException : BaseRosterdException
    {
        public ResourceDoesNotBelongToTenantException(List<string> messages) : base(messages)
        {
        }

        public ResourceDoesNotBelongToTenantException(string message) : base(new List<string> { message })
        {
        }
    }
}
