using System.Collections.Generic;
using PlayGen.SUGAR.Client.RequestQueue;

namespace PlayGen.SUGAR.Client.Development
{
	public class SUGARDevelopmentClient : SUGARClient
	{
		private DevelopmentClient _developmentClient;

		public DevelopmentClient Development => _developmentClient ?? (_developmentClient = new DevelopmentClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, RequestQueue, _evaluationNotifications));

		public SUGARDevelopmentClient(
			string baseAddress, 
			IRequestQueue requestQueue,
			IHttpHandler httpHandler = null,
			Dictionary<string, string> persistentHeaders = null, 
			Dictionary<string, string> sessionHeaders = null)
			: base(baseAddress, requestQueue, httpHandler, persistentHeaders, sessionHeaders)
		{
		}
	}
}
