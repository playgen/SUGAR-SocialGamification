using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using PlayGen.SUGAR.Contracts.Exceptions;

namespace PlayGen.SUGAR.Client.Exceptions
{
    public class ClientException : SUGARException
    {
		public HttpStatusCode StatusCode { get; }

	    public ClientException(HttpStatusCode statusCode)
	    {
		    StatusCode = statusCode;
	    }

	    public ClientException(HttpStatusCode statusCode, string message) : base(message)
	    {
			StatusCode = statusCode;
		}

		public ClientException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
	    {
			StatusCode = statusCode;
		}

		protected ClientException(HttpStatusCode statusCode, SerializationInfo info, StreamingContext context) : base(info, context)
	    {
			StatusCode = statusCode;
		}
	}
}
