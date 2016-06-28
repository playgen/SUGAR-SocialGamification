using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.WebAPI.Exceptions
{
	public class NullObjectException : Exception
	{
		public NullObjectException()
		{
		}

		public NullObjectException(string message)
			: base(message)
		{
		}

		public NullObjectException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
