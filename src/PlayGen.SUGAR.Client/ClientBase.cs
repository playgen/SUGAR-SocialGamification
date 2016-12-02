﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared.Web;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	public abstract class ClientBase
	{
        private static readonly Dictionary<string, string> PersistentHeaders = new Dictionary<string, string>();

        private readonly string _baseAddress;
		private readonly IHttpHandler _httpHandler;

	    protected readonly AsyncRequestController AsyncRequestController;
        protected readonly EvaluationNotifications EvaluationNotifications;

		public static readonly JsonSerializerSettings SerializerSettings;

		static ClientBase()
		{
			SerializerSettings = new JsonSerializerSettings()
			{
				//Formatting = Formatting.Indented,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			};
			SerializerSettings.Converters.Add(new StringEnumConverter());
		}

		protected ClientBase(string baseAddress, IHttpHandler httpHandler, AsyncRequestController asyncRequestController, EvaluationNotifications evaluationNotifications)
		{
			if (!(Uri.IsWellFormedUriString(baseAddress, UriKind.Absolute)))
			{
				throw new Exception("Base address is not an absolute or valid URI");
			}
			_baseAddress = baseAddress;
			_httpHandler = httpHandler;
		    AsyncRequestController = asyncRequestController;
            EvaluationNotifications = evaluationNotifications;
		}

	    protected static void EnableEvaluationNotifications(bool enable = true)
	    {
	        if (enable)
	        {
	            PersistentHeaders[HeaderKeys.EvaluationNotifications] = $"{enable}";
	        }
	        else
	        {
	            PersistentHeaders.Remove(HeaderKeys.EvaluationNotifications);
	        }
	    }

	    protected void ClearSessionData()
	    {
	        AsyncRequestController.Clear();
            PersistentHeaders.Clear();
	        EvaluationNotifications.Clear();
	    }

		protected bool AreUriParamsValid(object[] param)
		{
			return param == null || param.Length == 0 || param.All(pa => !string.IsNullOrEmpty(pa?.ToString()));
		}

		/// <summary>
		/// Get a UriBuilder object with the origin and web api path
		/// </summary>
		/// <param name="apiSuffix">WebAPI path relative to web origin, eg. /api</param>
		/// <param name="param">URI para</param>
		/// <returns></returns>
		protected UriBuilder GetUriBuilder(string apiSuffix, params object[] param)
		{
			Console.WriteLine("ClientBase::GetUriBuilder");

			if (!AreUriParamsValid(param))
			{
				throw new Exception("Passed values must not be empty or null");
			}

			var formattedUri = string.Format(apiSuffix, param);

			var separator = "";
			if (!(_baseAddress.EndsWith("/") || formattedUri.StartsWith("/")))
			{
				separator = "/";
			}
			return new UriBuilder(_baseAddress + separator + formattedUri);
		}

		private static string SerializePayload(object payload)
		{
			return payload == null ? string.Empty : JsonConvert.SerializeObject(payload, SerializerSettings);
		}

		private HttpRequest CreateRequest(string url, string method, IDictionary<string, string> headers, object payload)
		{
			var requestHeaders = headers == null ? new Dictionary<string, string>() : new Dictionary<string, string>(headers);

            foreach (var keyValuePair in PersistentHeaders)
            {
                requestHeaders[keyValuePair.Key] = keyValuePair.Value;
            }

			return new HttpRequest()
			{
				Url = url,
				Method = method,
				Headers = requestHeaders,
				Content = SerializePayload(payload)
			};
		}
		

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TResponse"></typeparam>
		/// <param name="response"></param>
		/// <returns></returns>
		private TResponse UnwrapResponse<TResponse>(HttpResponse response)
		{
			var wrappedResponse = JsonConvert.DeserializeObject<ResponseWrapper<TResponse>>(response.Content, SerializerSettings);
			var content = wrappedResponse.Response;

			EvaluationNotifications.Enqueue(wrappedResponse.EvaluationsProgress.ToNotifications());

			return content;
		}

		#region PostPut

		protected TResponse Post<TRequest, TResponse>(string url, TRequest payload, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			return PostPut<TResponse>(url, "POST", headers, payload, expectedStatusCodes);
		}

		protected void Post<TRequest>(string url, TRequest payload, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			PostPut(url, "POST", headers, payload, expectedStatusCodes);
		}

		protected TResponse Put<TRequest, TResponse>(string url, TRequest payload, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			return PostPut<TResponse>(url, "PUT", headers, payload, expectedStatusCodes);
		}

		protected void Put<TRequest>(string url, TRequest payload, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			PostPut(url, "PUT", headers, payload, expectedStatusCodes);
		}

		protected TResponse PostPut<TResponse>(string url, string method, Dictionary<string, string> headers, object payload, IEnumerable<HttpStatusCode> expectedStatusCodes)
		{
			var request = CreateRequest(url, method, headers, payload);
			var response = _httpHandler.HandleRequest(request);
			Console.WriteLine("ClientBase::PostPut::ProcessResponse");
			ProcessResponse(response, expectedStatusCodes ?? new [] { HttpStatusCode.OK });
			Console.WriteLine("ClientBase::PostPut::DeserializeResponse");

			return UnwrapResponse<TResponse>(response);
		}

		protected void PostPut(string url, string method, Dictionary<string, string> headers, object payload, IEnumerable<HttpStatusCode> expectedStatusCodes)
		{
			var request = CreateRequest(url, method, headers, payload);
			var response = _httpHandler.HandleRequest(request);
			ProcessResponse(response, expectedStatusCodes ?? new[] { HttpStatusCode.OK });
		}

		#endregion

		#region GetDelete

		protected TResponse Get<TResponse>(string url, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			return GetDelete<TResponse>(url, "GET", headers, expectedStatusCodes);
		}

		protected void Get(string url, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			GetDelete(url, "GET", headers, expectedStatusCodes);
		}

		protected TResponse Delete<TResponse>(string url, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			return GetDelete<TResponse>(url, "DELETE", headers, expectedStatusCodes);
		}

		protected void Delete(string url, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			GetDelete(url, "DELETE", headers, expectedStatusCodes);
		}

		protected TResponse GetDelete<TResponse>(string url, string method, Dictionary<string, string> headers, IEnumerable<HttpStatusCode> expectedStatusCodes)
		{
			var request = CreateRequest(url, method, headers, null);
			var response = _httpHandler.HandleRequest(request);
			ProcessResponse(response, expectedStatusCodes ?? new [] { HttpStatusCode.OK });
			return UnwrapResponse<TResponse>(response);
		}

		protected void GetDelete(string url, string method, Dictionary<string, string> headers, IEnumerable<HttpStatusCode> expectedStatusCodes)
		{
			var request = CreateRequest(url, method, headers, null);
			var response = _httpHandler.HandleRequest(request);
			ProcessResponse(response, expectedStatusCodes ?? new [] { HttpStatusCode.OK });
		}

		#endregion

		/// <summary>
		/// Inspect the web response status code, returns on success or throw.
		/// </summary>
		/// <param name="response"></param>
		/// <param name="expectedStatusCodes"></param>
		/// <exception cref="Exception">HTTP Status Code not equal to 200 (OK)</exception>
		private void ProcessResponse(HttpResponse response, IEnumerable<HttpStatusCode> expectedStatusCodes)
		{
			if (!expectedStatusCodes.Contains((HttpStatusCode)response.StatusCode))
			{
				var error = "API ERROR. Status Code: " + response.StatusCode + ".";
				if (!(response.StatusCode >= 200 && response.StatusCode <= 299))
				{
					error += " Message: " + response.Content;
				}
				throw new ClientException((HttpStatusCode)response.StatusCode, error);
			}

			//TODO: check if this has changed
			if (response.Headers.ContainsKey(HeaderKeys.Authorization))
			{
				PersistentHeaders[HeaderKeys.Authorization] = response.Headers[HeaderKeys.Authorization];
			}
		}
	}
}
