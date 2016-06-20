using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataController.Exceptions
{
    public class DbExceptionHandler
    {
        public void Handle(Exception exception)
        {
            string message = exception.GetBaseException().Message;

            if (message.ToLower().Contains("duplicate"))
            {
                throw new DuplicateRecordException(message);
            }
            else
            {
                throw exception;
            }
        }
    }
}
