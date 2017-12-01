using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.Server.WebAPI.Exceptions
{
    public class IncompatibleAPIVersionException : Exception
    {
		public IncompatibleAPIVersionException(string message) : base(message)
		{
		}

		public IncompatibleAPIVersionException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
