using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PlayGen.SUGAR.Client
{
	public class DefaultHttpHandler : IHttpHandler
	{
		protected WebRequest CreateRequest(HttpRequest request)
		{
			var webRequest = WebRequest.Create(request.Url);
			webRequest.Method = request.Method;
			foreach (var header in request.Headers)
			{
				webRequest.Headers[header.Key] = header.Value;
			}
			return webRequest;
		}

		public HttpResponse HandleRequest(HttpRequest request)
		{
			switch (request.Method.ToUpperInvariant())
			{
				case "GET":
				case "DELETE":
				case "POST":
				case "PUT":
					return ExecuteRequest(request);

				default:
					throw new NotImplementedException($"Request method '{request.Method}' not supported");
			}
		}

		/// <summary>
		/// Set the content stream and related properties of the specified WebRequest object with the byte array
		/// </summary>
		/// <param name="request"></param>
		/// <param name="payload"></param>
		protected void SendData(WebRequest request, byte[] payload)
		{
#if NET35
			request.ContentLength = payload.Length;
#endif
			request.ContentType = "application/json";
#if NET35
			var dataStream = request.GetRequestStream();
#else
			var dataStream = request.GetRequestStreamAsync().Result;
#endif
			dataStream.Write(payload, 0, payload.Length);
			dataStream.Dispose();
		}


		/// <summary>
		/// Create a WebRequest for the specified uri and HTTP verb
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public HttpResponse ExecuteRequest(HttpRequest request)
		{
			var webRequest = CreateRequest(request);

			if (!string.IsNullOrEmpty(request.Content))
			{
				var payloadBytes = Encoding.UTF8.GetBytes(request.Content);
				SendData(webRequest, payloadBytes);
			}

			HttpWebResponse webResponse;
			try
			{
#if NET35
				webResponse = (HttpWebResponse)webRequest.GetResponse();
#else
				webResponse = (HttpWebResponse)webRequest.GetResponseAsync().Result;
#endif
			}
			catch (WebException ex)
			{
				webResponse = (HttpWebResponse)ex.Response;
			}

			var response = new HttpResponse
			{
				StatusCode = (int)webResponse.StatusCode,
				Headers = webResponse.Headers.AllKeys.ToDictionary(k => k, v => webResponse.Headers[v])
			};

			var dataStream = webResponse.GetResponseStream();
			if (dataStream != null)// && dataStream.Length > 0)
			{
				var reader = new StreamReader(dataStream);
				response.Content = reader.ReadToEnd();
			}

			return response;
		}
	}
}
