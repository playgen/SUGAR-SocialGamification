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
			switch (request.Method.ToUpperInvariant())
			{
				case "GET":
				case "DELETE":
				case "POST":
				case "PUT":
					var requestString = JsonConvert.SerializeObject(request);
					var responseString = HttpRequest(requestString);
					return JsonConvert.DeserializeObject<HttpResponse>(responseString);

				default:
					throw new NotImplementedException($"Request method '{request.Method}' not supported");
			}
		}

		[DllImport("__Internal")]
		private static extern string HttpRequest(string requestString);

	}
}
