using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.RequestQueue;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Session specific operations.
	/// </summary>
	public class SessionClient : ClientBase
	{
		private const string ControllerPrefix = "api";

		public SessionClient(
			string baseAddress,
			IHttpHandler httpHandler,
			Dictionary<string, string> constantHeaders,
			Dictionary<string, string> sessionHeaders,
			IRequestQueue requestQueue,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, constantHeaders, sessionHeaders, requestQueue, evaluationNotifications)
		{
		}

		/// <summary>
		/// Logs in an account into a game based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="gameId">ID of the game the user is logging into.</param>
		/// <param name="account"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		public AccountResponse Login(int gameId, AccountRequest account)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/logingame", gameId).ToString();
			return Post<AccountRequest, AccountResponse>(query, account);
		}

		public void LoginAsync(int gameId, AccountRequest account, Action<AccountResponse> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => Login(gameId, account),
				onSuccess,
				onError);
		}

		// todo comment
		public AccountResponse CreateAndLogin(int gameId, AccountRequest accountRequest)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/createandlogingame", gameId).ToString();
			return Post<AccountRequest, AccountResponse>(query, accountRequest);
		}

		public void CreateAndLoginAsync(int gameId, AccountRequest account, Action<AccountResponse> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => CreateAndLogin(gameId, account),
				onSuccess,
				onError);
		}

		public void Heartbeat()
		{
			var query = GetUriBuilder(ControllerPrefix + "/heartbeat").ToString();
			Get(query);
		}

		public void HeartbeatAsync(Action onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(Heartbeat,
				onSuccess,
				onError);
		}

		// todo comment
		public void Logout()
		{
			var query = GetUriBuilder(ControllerPrefix + "/logout").ToString();
			Get(query);

			ClearSessionData();
		}

		public void LogoutAsync(Action onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(Logout,
				onSuccess,
				onError);
		}
	}
}
