using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class EntityAlreadyExistsException : BaseRosterdException
    {
        public EntityAlreadyExistsException(List<string> messages) : base(messages)
        {
        }

        public EntityAlreadyExistsException(string message) : base(new List<string> { message })
        {
        }
    }
}
