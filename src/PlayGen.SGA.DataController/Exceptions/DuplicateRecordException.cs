using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataController.Exceptions
{
    public class DuplicateRecordException : Exception
    {
        public DuplicateRecordException()
        {
        }

        public DuplicateRecordException(string message) 
            : base(message)
        {
        }

        public DuplicateRecordException(string message, Exception inner) 
            : base(message, inner)
        {   
        }
    }
}
