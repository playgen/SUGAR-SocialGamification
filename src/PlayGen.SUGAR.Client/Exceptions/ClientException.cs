using System;
using System.Net;
using PlayGen.SUGAR.Common.Shared.Exceptions;

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

        // SerializationInfo not currently supported by .netcore
		//protected ClientException(HttpStatusCode statusCode, SerializationInfo info, StreamingContext context) : base(info, context)
	 //   {
		//	StatusCode = statusCode;
		//}
	}
}
