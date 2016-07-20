using System;
using System.IO;
using System.Linq;
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

		protected bool IsURIParamsValid(object[] param)
		{
			foreach (var pa in param)
			{
				if (string.IsNullOrEmpty(pa.ToString()))
				{
					return false;
				}
			}

			return true;
		}

		protected UriBuilder GetUriBuilder(string apiSuffix, params object[] param)
		{
			if (!IsURIParamsValid(param))
			{
				throw new Exception("Passed values must not be empty or null");
			}

			var formattedURI = string.Format(apiSuffix, param);

			var separator = "";
			if (!(_baseAddress.EndsWith("/") || formattedURI.StartsWith("/")))
			{
				separator = "/";
			}
			return new UriBuilder(_baseAddress + separator + formattedURI);
		}

		protected TResponse Get<TResponse>(string uri, HttpStatusCode[] acceptableStatusCodes = null)
		{
			var request = CreateRequest(uri, "GET");
			HttpWebResponse response;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{
				response = (HttpWebResponse)ex.Response;
			}
			ProcessResponse(response, acceptableStatusCodes ?? new HttpStatusCode[] { HttpStatusCode.OK });
			return GetResponse<TResponse>(response);
		}

		protected TResponse Post<TRequest, TResponse>(string url, TRequest payload)
		{
			var response = PostPut(url, payload, "POST");
			ProcessResponse(response, new HttpStatusCode[] { HttpStatusCode.OK });
			return GetResponse<TResponse>(response);
		}

		protected void Post<TRequest>(string url, TRequest payload)
		{
			var response = PostPut(url, payload, "POST");
			ProcessResponse(response, new HttpStatusCode[] { HttpStatusCode.OK });
		}

		protected TResponse Put<TRequest, TResponse>(string url, TRequest payload)
		{
			var response = PostPut(url, payload, "PUT");
			ProcessResponse(response, new HttpStatusCode[] { HttpStatusCode.OK });
			return GetResponse<TResponse>(response);
		}

		protected void Put<TRequest>(string url, TRequest payload)
		{
			var response = PostPut(url, payload, "PUT");
			ProcessResponse(response, new HttpStatusCode[] { HttpStatusCode.OK });
		}

		protected TResponse Delete<TResponse>(string url)
		{
			var response = DeleteRequest(url);
			ProcessResponse(response, new HttpStatusCode[] { HttpStatusCode.OK });
			return GetResponse<TResponse>(response);
		}

		protected void Delete(string url)
		{
			var response = DeleteRequest(url);
			ProcessResponse(response, new HttpStatusCode[] { HttpStatusCode.OK });
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
				return default(TResponse);
			}
			var reader = new StreamReader(dataStream);
			return JsonConvert.DeserializeObject<TResponse>(reader.ReadToEnd());
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
			request.Headers.Add("Authorization", _credentials.Authorization);
			return request;
		}

		/// <summary>
		/// Inspect the web response status code, returns on success or throw.
		/// </summary>
		/// <param name="response"></param>
		/// <exception cref="Exception">HTTP Status Code not equal to 200 (OK)</exception>
		private void ProcessResponse(HttpWebResponse response, HttpStatusCode[] expectedStatusCode)
		{
			if (!expectedStatusCode.Contains(response.StatusCode))
			{
				var error = "API ERROR. Status Code: " + response.StatusCode + ".";
				if (!((int)response.StatusCode >= 200 && (int)response.StatusCode <= 299))
				{
					using (var reader = new StreamReader(response.GetResponseStream()))
					{
						error += " Message: " + reader.ReadToEnd();
					}
				}
				throw new Exception(error);
			}

			_credentials.Authorization = response.Headers["Authorization"];
		}

		private HttpWebResponse PostPut<TRequest>(string url, TRequest payload, string method)
		{
			var payloadString = JsonConvert.SerializeObject(payload);
			var payloadBytes = Encoding.UTF8.GetBytes(payloadString);
			var request = CreateRequest(url, method);
			SendData(request, payloadBytes);
			HttpWebResponse response;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{
				response = (HttpWebResponse)ex.Response;
			}
			
			return response;
		}

		private HttpWebResponse DeleteRequest(string url)
		{
			var request = CreateRequest(url, "DELETE");
			HttpWebResponse response;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{
				response = (HttpWebResponse)ex.Response;
			}

			return response;
		}
	}
}
