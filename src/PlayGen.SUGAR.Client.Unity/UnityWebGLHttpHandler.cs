using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace PlayGen.SUGAR.Client.Unity
{
	public class UnityWebGlHttpHandler : IHttpHandler
	{
		public UnityWebGlHttpHandler()
		{
			var c = new ReferenceConverter(typeof(UnityWebGlHttpHandler));
		}

		public HttpResponse HandleRequest(HttpRequest request)
		{
			switch (request.Method.ToUpperInvariant())
			{
				case "GET":
				case "DELETE":
				case "POST":
				case "PUT":
					var requestString = JsonConvert.SerializeObject(request, ClientBase.SerializerSettings);
					var responseString = HttpRequest(requestString);
					return JsonConvert.DeserializeObject<HttpResponse>(responseString, ClientBase.SerializerSettings);

				default:
					throw new NotImplementedException($"Request method '{request.Method}' not supported");
			}
		}

		[DllImport("__Internal")]
		private static extern string HttpRequest(string requestString);
	}
}