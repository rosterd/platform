using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class Auth0IdNotSetException : BaseRosterdException
    {
        public Auth0IdNotSetException(List<string> messages) : base(messages)
        {
        }

        public Auth0IdNotSetException(string message) : base(new List<string> { message })
        {
        }
    }
}
