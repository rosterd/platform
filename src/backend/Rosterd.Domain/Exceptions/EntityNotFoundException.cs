using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class EntityNotFoundException : BaseRosterdException
    {
        public EntityNotFoundException(List<string> messages) : base(messages)
        {
        }

        public EntityNotFoundException(string message) : base(new List<string> { message })
        {
        }
    }
}
