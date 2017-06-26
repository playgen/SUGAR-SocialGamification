using System;
using System.Net;
using PlayGen.SUGAR.Common.Exceptions;

namespace PlayGen.SUGAR.Client.Exceptions
{
	public class ClientHttpException : SUGARException
	{
		public ClientHttpException(HttpStatusCode statusCode)
		{
			StatusCode = statusCode;
		}

		public ClientHttpException(HttpStatusCode statusCode, string message) : base(message)
		{
			StatusCode = statusCode;
		}

		public ClientHttpException(HttpStatusCode statusCode, string message, Exception innerException) : base(message,
			innerException)
		{
			StatusCode = statusCode;
		}

		public HttpStatusCode StatusCode { get; }
	}
}