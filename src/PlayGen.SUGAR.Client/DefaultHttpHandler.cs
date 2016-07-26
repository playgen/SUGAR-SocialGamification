using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace PlayGen.SUGAR.Client
{
    public class DefaultHttpHandler : IHttpHandler
    {
		private WebRequest CreateRequest(HttpRequest request)
		{
			Console.WriteLine("ClientBase::CreateRequest");

			var webRequest = WebRequest.Create(request.Url);
			webRequest.Method = request.Method;
			foreach (var header in request.Headers)
			{
				webRequest.Headers.Add(header.Key, header.Value);
			}
			return webRequest;
		}

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
			var webRequest = CreateRequest(request);
			HttpWebResponse webResponse;
			try
			{
				webResponse = (HttpWebResponse)webRequest.GetResponse();
			}
			catch (WebException ex)
			{
				webResponse = (HttpWebResponse)ex.Response;
			}

			var response = new HttpResponse()
			{
				StatusCode = (int)webResponse.StatusCode,
				Headers = webResponse.Headers.AllKeys.ToDictionary(k => k, v => webResponse.Headers[v]),
			};

			var dataStream = webResponse.GetResponseStream();
			if (dataStream != null && webResponse.ContentLength > 0)
			{
				var reader = new StreamReader(dataStream);
				response.Content = reader.ReadToEnd();
			}

			return response;
		}


		/// <summary>
		/// Set the content stream and related properties of the specified WebRequest object with the byte array
		/// </summary>
		/// <param name="request"></param>
		/// <param name="payload"></param>
		private void SendData(WebRequest request, byte[] payload)
		{
			Console.WriteLine("ClientBase::SendData");

			request.ContentLength = payload.Length;
			request.ContentType = "application/json";
			Console.WriteLine("ClientBase::SendData [GetRequestStream]");

			var dataStream = request.GetRequestStream();
			Console.WriteLine("ClientBase::SendData [Write]");
			dataStream.Write(payload, 0, payload.Length);
			Console.WriteLine("ClientBase::SendData [CloseStream]");

			dataStream.Close();
		}


	    /// <summary>
	    /// Create a WebRequest for the specified uri and HTTP verb
	    /// </summary>
	    /// <param name="request"></param>
	    /// <returns></returns>
	    public HttpResponse PostPut(HttpRequest request)
		{
			var payloadBytes = Encoding.UTF8.GetBytes(request.Content);

			var webRequest = CreateRequest(request);
			SendData(webRequest, payloadBytes);

			HttpWebResponse webResponse;
			try
			{
				webResponse = (HttpWebResponse)webRequest.GetResponse();
			}
			catch (WebException ex)
			{
				webResponse = (HttpWebResponse)ex.Response;
			}

			var response = new HttpResponse()
			{
				StatusCode = (int)webResponse.StatusCode,
				Headers = webResponse.Headers.AllKeys.ToDictionary(k => k, v => webResponse.Headers[v]),
			};

			var dataStream = webResponse.GetResponseStream();
			if (dataStream != null && webResponse.ContentLength > 0)
			{
				var reader = new StreamReader(dataStream);
				response.Content = reader.ReadToEnd();
			}

			return response;
		}
	}
}
