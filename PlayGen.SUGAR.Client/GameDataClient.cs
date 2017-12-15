using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Client.RequestQueue;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates GameData specific operations.
	/// </summary>
	public class GameDataClient : ClientBase
	{
		private const string ControllerPrefix = "api/gamedata";

		public GameDataClient(
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
		/// Find a list of all GameData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="EvaluationDataResponse"/> which match the search criteria.</returns>
		public IEnumerable<EvaluationDataResponse> Get(int actorId, int gameId, string[] key)
		{
			var query = GetUriBuilder(ControllerPrefix)
				.AppendQueryParameter(actorId, "actorId={0}")
				.AppendQueryParameter(gameId, "gameId={0}")
				.AppendQueryParameters(key, "key={0}")
				.ToString();
			return Get<IEnumerable<EvaluationDataResponse>>(query);
		}

		public void GetAsync(int actorId, int gameId, string[] key, Action<IEnumerable<EvaluationDataResponse>> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => Get(actorId, gameId, key),
				onSuccess,
				onError);
		}

		/// <summary>
		///  Find a list of all Actors that have data saved for the game <param name="id"/> provided.
		/// </summary>
		/// <param name="id">ID of a Game.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		public IEnumerable<ActorResponse> GetGameActors(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/gameactors/{0}", id).ToString();
			return Get<IEnumerable<ActorResponse>>(query);
		}

		public void GetGameActorsAsync(int id, Action<IEnumerable<ActorResponse>> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => GetGameActors(id),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Finds a list of GameData with the highest <param name="dataType"/> for each <param name="key"/> provided that matches the <param name="actorId"/> and <param name="gameId"/>.
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <param name="dataType">Data type of value</param>
		/// <returns>A list of <see cref="EvaluationDataResponse"/> which match the search criteria.</returns>
		public EvaluationDataResponse GetByLeaderboardType(int actorId, int gameId, string key, EvaluationDataType dataType, LeaderboardType leaderboardType)
		{
			var query = GetUriBuilder(ControllerPrefix + "/leaderboardType/{0}/{1}/{2}/{3}/{4}", actorId, gameId, key, dataType.ToString(), leaderboardType.ToString()).ToString();
			return Get<EvaluationDataResponse>(query);
		}

		public void GetByLeaderboardTypeAsync(int actorId, int gameId, string key, EvaluationDataType dataType, LeaderboardType leaderboardType, Action<EvaluationDataResponse> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => GetByLeaderboardType(actorId, gameId, key, dataType, leaderboardType),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Create a new GameData record.
		/// </summary>
		/// <param name="data"><see cref="EvaluationDataRequest"/> object that holds the details of the new GameData.</param>
		/// <returns>A <see cref="EvaluationDataResponse"/> containing the new GameData details.</returns>
		public EvaluationDataResponse Add(EvaluationDataRequest data)
		{
			if (data.Key == null)
			{
				throw new ArgumentException("No Key provided. Keys must be non-empty strings containing only alpha-numeric characters and underscores.");
			}
			if (!RegexUtil.IsAlphaNumericUnderscoreNotEmpty(data.Key))
			{
				throw new ArgumentException($"Invalid Key {data.Key}. Keys must be non-empty strings containing only alpha-numeric characters and underscores.");
			}

			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Post<EvaluationDataRequest, EvaluationDataResponse>(query, data);
		}

		public void AddAsync(EvaluationDataRequest data, Action<EvaluationDataResponse> onSuccess, Action<Exception> onError)
		{
			RequestQueue.EnqueueRequest(() => Add(data), 
				onSuccess, 
				onError);
		}
	}
}
