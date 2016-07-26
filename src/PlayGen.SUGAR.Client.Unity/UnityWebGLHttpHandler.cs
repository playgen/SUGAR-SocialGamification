using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
					return GetDelete(request);

				case "POST":
				case "PUT":
					return PostPut(request);

				default:
					throw new NotImplementedException($"Request method '{request.Method}' not supported");
			}
	    }

	    public HttpResponse GetDelete(HttpRequest request)
	    {
			throw new NotImplementedException();
			var requestString = JsonConvert.SerializeObject(request);
			Application.ExternalCall("xxx", requestString);
	    }

	    public HttpResponse PostPut(HttpRequest request)
	    {
		    throw new NotImplementedException();
			var requestString = JsonConvert.SerializeObject(request);
			Application.ExternalCall("xxx", requestString);
		}
	}
}
