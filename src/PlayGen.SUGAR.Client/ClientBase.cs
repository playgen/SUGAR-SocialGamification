using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace PlayGen.SUGAR.Client
{
	public abstract class ClientBase
	{
		private readonly string _baseAddress;
		private readonly Credentials _credentials;
		
		protected ClientBase(string baseAddress, Credentials credentials)
		{
			if (!(Uri.IsWellFormedUriString(baseAddress, UriKind.Absolute)))
			{
				throw new Exception("Base address is not an absolute or valid URI");
			}
			_baseAddress = baseAddress;
			_credentials = credentials;
		}
		
		/// <summary>
		/// Get a UriBuilder object with the origin and web api path
		/// </summary>
		/// <param name="apiSuffix">WebAPI path relative to web origin, eg. /api</param>
		/// <returns></returns>
		protected UriBuilder GetUriBuilder(string apiSuffix)
		{
			var separator = "";
			if (!(_baseAddress.EndsWith("/") || apiSuffix.StartsWith("/")))
			{
				separator = "/";
			}
			return new UriBuilder(_baseAddress + separator + apiSuffix);
		}

		/// <summary>
		/// Create a WebRequest for the specified uri and HTTP verb
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="method">HTTP verb (GET or DELETE)</param>
		/// <returns></returns>
		private WebRequest CreateRequest(string uri, string method)
		{
			var request = WebRequest.Create(uri);
			request.Method = method;
			request.Headers.Add("Bearer", _credentials.Token);
			return request;
		}

		/// <summary>
		/// Set the content stream and related properties of the specified WebRequest object with the byte array
		/// </summary>
		/// <param name="request"></param>
		/// <param name="payload"></param>
		private static void SendData(WebRequest request, byte[] payload)
		{
			request.ContentLength = payload.Length;
			request.ContentType = "application/json";
			var dataStream = request.GetRequestStream();
			dataStream.Write(payload, 0, payload.Length);
			dataStream.Close();
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="response"></param>
		/// <returns></returns>
		private static TResponse GetResponse<TResponse>(WebResponse response)
		{
			var dataStream = response.GetResponseStream();
			if (dataStream == null || response.ContentLength == 0)
			{
				throw new Exception("Response was empty :(");
			} 
			var reader = new StreamReader(dataStream);
			return JsonConvert.DeserializeObject<TResponse>(reader.ReadToEnd());
		}

		/// <summary>
		/// Inspect the web response status code, returns on success or throw.
		/// </summary>
		/// <param name="response"></param>
		/// <exception cref="Exception">HTTP Status Code not equal to 200 (OK)</exception>
		private void ProcessResponse(HttpWebResponse response)
		{
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception("API ERROR, Status Code: " + response.StatusCode + ". Message: " + response.StatusDescription);
			}

			_credentials.Token = response.Headers["Bearer"];
		}

		protected TResponse Get<TResponse>(string uri)
		{
			var request = CreateRequest(uri, "GET");
			var response = (HttpWebResponse)request.GetResponse();
			ProcessResponse(response);
			return GetResponse<TResponse>(response);
		}

		protected TResponse Post<TRequest, TResponse>(string url, TRequest payload)
		{
			var response = PostPut(url, payload, "POST");
			return GetResponse<TResponse>(response);
		}

		protected void Post<TRequest>(string url, TRequest payload)
		{
			PostPut(url, payload, "POST");
		}

		protected TResponse Put<TRequest, TResponse>(string url, TRequest payload)
		{
			var response = PostPut(url, payload, "PUT");
			return GetResponse<TResponse>(response);
		}

		protected void Put<TRequest>(string url, TRequest payload)
		{
			PostPut(url, payload, "PUT");
		}

		private HttpWebResponse PostPut<TRequest>(string url, TRequest payload, string method)
		{
			var payloadString = JsonConvert.SerializeObject(payload);
			var payloadBytes = Encoding.UTF8.GetBytes(payloadString);
			var request = CreateRequest(url, method);
			SendData(request, payloadBytes);
			var response = (HttpWebResponse)request.GetResponse();
			ProcessResponse(response);
			return response;
		}

		protected TResponse Delete<TResponse>(string url)
		{
			var response = DeleteRequest(url);
			return GetResponse<TResponse>(response);
		}

		protected void Delete(string url)
		{
			DeleteRequest(url);
		}

		private HttpWebResponse DeleteRequest(string url)
		{
			var request = CreateRequest(url, "DELETE");
			var response = (HttpWebResponse)request.GetResponse();
			ProcessResponse(response);
			return response;
		}
	}
}
