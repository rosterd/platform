using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class RoleDoesNotExistException : BaseRosterdException
    {
        public RoleDoesNotExistException(List<string> messages) : base(messages)
        {
        }

        public RoleDoesNotExistException(string message) : base(new List<string> { message })
        {
        }
    }
}
