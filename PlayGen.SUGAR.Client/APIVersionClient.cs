using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;

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
			IAsyncRequestController asyncRequestController,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, constantHeaders, sessionHeaders, asyncRequestController, evaluationNotifications)
		{
		}
		
		public string Get()
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Get<string>(query);
		}

		public void GetAsync(Action<string> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(Get,
				onSuccess,
				onError);
		}
	}
}
