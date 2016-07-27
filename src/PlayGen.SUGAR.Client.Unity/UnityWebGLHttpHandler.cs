using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using PlayGen.SUGAR.Client;
using Newtonsoft.Json;
using UnityEngine;


namespace PlayGen.SUGAR.Client.Unity
{
	public class UnityWebGlHttpHandler : IHttpHandler
	{
		public HttpResponse HandleRequest(HttpRequest request)
		{
			Console.WriteLine("UnityWebGlHttpHandler::HandleRequest");

			switch (request.Method.ToUpperInvariant())
			{
				case "GET":
				case "DELETE":
				case "POST":
				case "PUT":
					Console.WriteLine("UnityWebGlHttpHandler::HandleRequest::1");
					var requestString = JsonConvert.SerializeObject(request, ClientBase.SerializerSettings);
					Console.WriteLine("UnityWebGlHttpHandler::HandleRequest::requestString::" + requestString);
					var responseString = HttpRequest(requestString);
					Console.WriteLine("UnityWebGlHttpHandler::HandleRequest::responseString::" + responseString);
					return JsonConvert.DeserializeObject<HttpResponse>(responseString, ClientBase.SerializerSettings);

				default:
					Console.WriteLine("UnityWebGlHttpHandler::HandleRequest::4");
					throw new NotImplementedException($"Request method '{request.Method}' not supported");
			}
		}

		[DllImport("__Internal")]
		private static extern string HttpRequest(string requestString);

	}
}
