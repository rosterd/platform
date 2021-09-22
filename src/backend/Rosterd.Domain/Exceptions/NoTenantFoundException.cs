using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class NoTenantFoundException : BaseRosterdException
    {
        public NoTenantFoundException(List<string> messages) : base(messages)
        {
        }

        public NoTenantFoundException(string message) : base(new List<string> { message })
        {
        }
    }
}
