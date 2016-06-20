using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlayGen.SGA.WebAPI.Exceptions
{
    public class InvalidAccountDetailsException : Exception
    {
        public InvalidAccountDetailsException()
        {
        }

        public InvalidAccountDetailsException(string message) 
            : base(message)
        {
        }

        public InvalidAccountDetailsException(string message, Exception inner) 
            : base(message, inner)
        {   
        }
    }
}
