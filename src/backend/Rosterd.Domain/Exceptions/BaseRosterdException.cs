using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class BaseRosterdException : Exception
    {
        public readonly List<string> Messages;

        public BaseRosterdException(List<string> messages) => Messages = messages;
    }
}
