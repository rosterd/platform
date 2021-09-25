using System;
using System.Collections.Generic;

namespace Rosterd.Domain.Exceptions
{
    public class BadRequestException : BaseRosterdException
    {
        public BadRequestException(List<string> messages) : base(messages)
        {
        }

        public BadRequestException(string message) : base(new List<string> { message })
        {
        }
    }
}
