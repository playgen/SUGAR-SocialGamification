using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.RequestQueue;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates API version specific operations.
	/// </summary>
	public class APIVersionClient : ClientBase
	{
		private const string ControllerPrefix = "api/version";

		public APIVersionClient(
			string baseAddress,
			IHttpHandler httpHandler,
			Dictionary<string, string> constantHeaders,
			Dictionary<string, string> sessionHeaders,
			IRequestQueue requestQueue,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, constantHeaders, sessionHeaders, requestQueue, evaluationNotifications)
		{
		}
		
		public string Get()
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Get<string>(query);
		}

		public void GetAsync(Action<string> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(Get,
				onSuccess,
				onError);
		}
	}
}
