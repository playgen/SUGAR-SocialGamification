using System.Collections.Generic;

namespace PlayGen.SUGAR.Client.Development
{
	public class SUGARDevelopmentClient : SUGARClient
	{
		private DevelopmentClient _developmentClient;

		public DevelopmentClient Development => _developmentClient ?? (_developmentClient = new DevelopmentClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));

		public SUGARDevelopmentClient(string baseAddress, IHttpHandler httpHandler = null, bool asyncEnabled = true, Dictionary<string, string> persistentHeaders = null, Dictionary<string, string> sessionHeaders = null, int timeoutMilliseconds = 60 * 1000)
			: base(baseAddress, httpHandler, asyncEnabled, persistentHeaders, sessionHeaders, timeoutMilliseconds)
		{
		}
	}
}
