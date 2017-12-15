using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.RequestQueue;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Game specific operations.
	/// </summary>
	public class GameClient : ClientBase
	{
		private const string ControllerPrefix = "api/game";

		public GameClient(
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
		/// Get a list of all Games.
		/// </summary>
		/// <returns>A list of <see cref="GameResponse"/> that hold Games details.</returns>
		public IEnumerable<GameResponse> Get()
		{
			var query = GetUriBuilder(ControllerPrefix + "/list").ToString();
			return Get<IEnumerable<GameResponse>>(query);
		}

		public void GetAsync(Action<IEnumerable<GameResponse>> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(Get,
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get a list of Games that match <param name="name"/> provided.
		/// </summary>
		/// <param name="name">Game name</param>
		/// <returns>A list of <see cref="GameResponse"/> which match the search criteria.</returns>
		public IEnumerable<GameResponse> Get(string name)
		{
			var query = GetUriBuilder(ControllerPrefix + "/find/{0}", name).ToString();
			
			return Get<IEnumerable<GameResponse>>(query);
		}

		public void GetAsync(string name, Action<IEnumerable<GameResponse>> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => Get(name),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Get Game that matches <param name="id"/> provided.
		/// </summary>
		/// <param name="id">Game id</param>
		/// <returns><see cref="GameResponse"/> which matches search criteria.</returns>
		public GameResponse Get(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/findbyid/{0}", id).ToString();
			return Get<GameResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		public void GetAsync(int id, Action<GameResponse> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => Get(id),
				onSuccess,
				onError);
		}
	}
}
