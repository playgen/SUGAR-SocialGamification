using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PlayGen.SUGAR.Client
{
	public class DefaultHttpHandler : IHttpHandler
	{
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

		private WebRequest CreateRequest(HttpRequest request)
		{
			var webRequest = WebRequest.Create(request.Url);
			webRequest.Method = request.Method;
			foreach (var header in request.Headers)
				webRequest.Headers.Add(header.Key, header.Value);
			return webRequest;
		}

		/// <summary>
		///     Set the content stream and related properties of the specified WebRequest object with the byte array
		/// </summary>
		/// <param name="request"></param>
		/// <param name="payload"></param>
		private void SendData(WebRequest request, byte[] payload)
		{
			request.ContentLength = payload.Length;
			request.ContentType = "application/json";
			var dataStream = request.GetRequestStream();
			dataStream.Write(payload, 0, payload.Length);
			dataStream.Close();
		}


		/// <summary>
		///     Create a WebRequest for the specified uri and HTTP verb
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
				webResponse = (HttpWebResponse) webRequest.GetResponse();
			}
			catch (WebException ex)
			{
				webResponse = (HttpWebResponse) ex.Response;
			}

			var response = new HttpResponse
			{
				StatusCode = (int) webResponse.StatusCode,
				Headers = webResponse.Headers.AllKeys.ToDictionary(k => k, v => webResponse.Headers[v])
			};

			var dataStream = webResponse.GetResponseStream();
			if (dataStream != null) // && dataStream.Length > 0)
			{
				var reader = new StreamReader(dataStream);
				response.Content = reader.ReadToEnd();
			}

			return response;
		}
	}
}