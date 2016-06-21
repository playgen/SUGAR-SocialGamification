using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataController.Exceptions
{
    public class MissingRecordException : Exception
    {
        public MissingRecordException()
        {
        }

        public MissingRecordException(string message) 
            : base(message)
        {
        }

        public MissingRecordException(string message, Exception inner) 
            : base(message, inner)
        {
        }
    }
}
