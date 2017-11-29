using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace PlayGen.SUGAR.Client.Tests
{
	public class HttpClientHandler : IHttpHandler
	{
		private readonly HttpClient _client;

		public HttpClientHandler(HttpClient client)
		{
			_client = client;
		}

		public HttpResponse HandleRequest(HttpRequest request)
		{
			var requestMessage = new HttpRequestMessage
			{
				RequestUri = new Uri(request.Url),
				Method = new HttpMethod(request.Method),
				Content = new StringContent(request.Content,
					Encoding.UTF8,
					"application/json")
			};

			request.Headers.ToList().ForEach(hkvp => requestMessage.Headers.Add(hkvp.Key, hkvp.Value));

			var responseMessage = _client.SendAsync(requestMessage).Result;
			
			var response = new HttpResponse
			{
				StatusCode = (int)responseMessage.StatusCode,
				Headers = responseMessage.Headers
					.ToList()
					.ToDictionary(
						kvp => kvp.Key,
						kvp => string.Join(",", kvp.Value)),
				Content = responseMessage.Content.ReadAsStringAsync().Result
			};

			return response;
		}
	}
}
