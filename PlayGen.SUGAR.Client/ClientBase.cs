using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Extensions;
using PlayGen.SUGAR.Common.Web;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	public abstract class ClientBase
	{
		private readonly Dictionary<string, string> _constantHeaders;
		private readonly Dictionary<string, string> _sessionHeaders;
		private readonly string _baseAddress;
		private readonly IHttpHandler _httpHandler;

		protected readonly IAsyncRequestController AsyncRequestController;
		protected readonly EvaluationNotifications EvaluationNotifications;

		public static readonly JsonSerializerSettings SerializerSettings;
		private string baseAddress;
		private IHttpHandler httpHandler;
		private Dictionary<string, string> constantHeaders;

		static ClientBase()
		{
			SerializerSettings = new JsonSerializerSettings
			{
				//Formatting = Formatting.Indented,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			};

			SerializerSettings.Converters.Add(new StringEnumConverter());
		}

		protected ClientBase(
			string baseAddress, 
			IHttpHandler httpHandler,
			Dictionary<string, string> constantHeaders,
			Dictionary<string, string> sessionHeaders,
			IAsyncRequestController asyncRequestController, 
			EvaluationNotifications evaluationNotifications)
		{
			if (!Uri.IsWellFormedUriString(baseAddress, UriKind.Absolute))
			{
				throw new ClientException("Base address is not an absolute or valid URI");
			}

			_baseAddress = baseAddress;
			_httpHandler = httpHandler;
			_constantHeaders = constantHeaders;
			_sessionHeaders = sessionHeaders;
			AsyncRequestController = asyncRequestController;
			EvaluationNotifications = evaluationNotifications;
		}
		
		protected void EnableEvaluationNotifications(bool enable = true)
		{
			if (enable)
			{
				_sessionHeaders[HeaderKeys.EvaluationNotifications] = $"{true}";
			}
			else
			{
				_sessionHeaders.Remove(HeaderKeys.EvaluationNotifications);
			}
		}

		protected void ClearSessionData()
		{
			AsyncRequestController.Clear();
			_sessionHeaders.Clear();
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
			if (!AreUriParamsValid(param))
			{
				throw new ClientException("Passed values must not be empty or null");
			}

			var formattedParams = FormatUriParameters(param);
			var formattedUri = string.Format(apiSuffix, formattedParams);

			var separator = "";
			if (!(_baseAddress.EndsWith("/") || formattedUri.StartsWith("/")))
			{
				separator = "/";
			}
			return new UriBuilder(_baseAddress + separator + formattedUri);
		}

		private static object[] FormatUriParameters(params object[] parameters)
		{
			var formattedParameters = new object[parameters.Length];

			for (var i = 0; i < parameters.Length; i++)
			{
				var parameter = parameters[i];
				object formattedParameter;

				if (parameter is DateTime time)
				{
					formattedParameter = time.SerializeToString();
				}
				else
				{
					formattedParameter = parameter;
				}

				formattedParameters[i] = formattedParameter;

			}

			return formattedParameters;
		}

		private static string SerializePayload(object payload)
		{
			return payload == null ? string.Empty : JsonConvert.SerializeObject(payload, SerializerSettings);
		}

		private HttpRequest CreateRequest(string url, string method, IDictionary<string, string> headers, object payload)
		{
			var requestHeaders = headers == null ? new Dictionary<string, string>() : new Dictionary<string, string>(headers);

			foreach (var keyValuePair in _constantHeaders)
			{
				requestHeaders[keyValuePair.Key] = keyValuePair.Value;
			}

			foreach (var keyValuePair in _sessionHeaders)
			{
				requestHeaders[keyValuePair.Key] = keyValuePair.Value;
			}

			return new HttpRequest
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

		protected TResponse Post<TResponse>(string url, object payload, IEnumerable<HttpStatusCode> expectedStatusCodes = null, Dictionary<string, string> headers = null)
		{
			return PostPut<TResponse>(url, "POST", headers, payload, expectedStatusCodes);
		}

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
			ProcessResponse(response, expectedStatusCodes ?? new [] { HttpStatusCode.OK });

			return UnwrapResponse<TResponse>(response);
		}

		protected void PostPut(string url, string method, Dictionary<string, string> headers, object payload, IEnumerable<HttpStatusCode> expectedStatusCodes)
		{
			var request = CreateRequest(url, method, headers, payload);
			var response = _httpHandler.HandleRequest(request);
			ProcessResponse(response, expectedStatusCodes ?? new[] { HttpStatusCode.OK });
		}

		protected string GetAuthorizationHeader()
		{
			return _sessionHeaders[HeaderKeys.Authorization];
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
				throw new ClientHttpException((HttpStatusCode)response.StatusCode, error);
			}

			var authorizationHeader = response.Headers.Keys.FirstOrDefault(h => string.Equals(h, HeaderKeys.Authorization, StringComparison.OrdinalIgnoreCase));

			if (authorizationHeader != null)
			{
				_sessionHeaders[HeaderKeys.Authorization] = response.Headers[authorizationHeader];
			}
		}
	}
}
