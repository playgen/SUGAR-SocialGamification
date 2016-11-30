using System;

using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Session specific operations.
	/// </summary>
	public class SessionClient : ClientBase
	{
		private const string ControllerPrefix = "api";

		public SessionClient(string baseAddress, IHttpHandler httpHandler, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, evaluationNotifications)
		{
		}

		/// <summary>
		/// Logs in an account into the system based on the name and password combination.
		/// Returns a JsonWebToken used for authorization in any further calls to the API.
		/// </summary>
		/// <param name="account"><see cref="AccountRequest"/> object that contains the account details provided.</param>
		/// <returns>A <see cref="AccountResponse"/> containing the Account details.</returns>
		public AccountResponse Login(AccountRequest account)
		{
			var query = GetUriBuilder(ControllerPrefix + "/login").ToString();
			return Post<AccountRequest, AccountResponse>(query, account);
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
			var query = GetUriBuilder(ControllerPrefix + "/{0}/login", gameId).ToString();
			return Post<AccountRequest, AccountResponse>(query, account);
		}

		public void LoginAsync(int gameId, AccountRequest account, Action<AccountResponse> success, Action<Exception> error)
		{
			try
			{
				var result = Login(gameId, account);
				success(result);
			}
			catch (Exception e)
			{
				error(e);
			}
		}

		// todo comment
		public AccountResponse CreateAndLogin(AccountRequest accountRequest)
		{
			var query = GetUriBuilder(ControllerPrefix + "/createandlogin").ToString();
			return Post<AccountRequest, AccountResponse>(query, accountRequest);
		}

		// todo comment
		public AccountResponse CreateAndLogin(int gameId, AccountRequest accountRequest)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/createandlogin", gameId).ToString();
			return Post<AccountRequest, AccountResponse>(query, accountRequest);
		}

		public void Heartbeat()
		{
			var query = GetUriBuilder(ControllerPrefix + "/heartbeat").ToString();
			Get(query);
		}

		// todo comment
		public void Logout()
		{
			var query = GetUriBuilder(ControllerPrefix + "/logout").ToString();
			Get(query);
		}
	}
}
