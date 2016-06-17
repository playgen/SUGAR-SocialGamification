using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlayGen.SGA.WebAPI.Exceptions
{
    public class InvalidLoginDetailsException : Exception
    {
        public InvalidLoginDetailsException()
        {
        }

        public InvalidLoginDetailsException(string message) 
            : base(message)
        {
        }

        public InvalidLoginDetailsException(string message, Exception inner) 
            : base(message, inner)
        {   
        }
    }
}
