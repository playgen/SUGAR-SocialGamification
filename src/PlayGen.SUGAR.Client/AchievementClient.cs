using System.Collections.Generic;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Achievement specific operations.
	/// </summary>
	public class AchievementClient : ClientBase
	{
		public AchievementClient(string baseAddress, Credentials credentials, IHttpHandler httpHandler) 
			: base(baseAddress, credentials, httpHandler)
		{
		}

		/// <summary>
		/// Find a Global Achievement that matches <param name="token"/>.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <returns>Returns <see cref="EvaluationResponse"/> that holds Achievement details</returns>
		public EvaluationResponse GetGlobalById(string token)
		{
			var query = GetUriBuilder("api/achievements/find/{0}/global", token).ToString();
			return Get<EvaluationResponse>(query, expectedStatusCodes: new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Find a Achievement that matches <param name="token"/> and <param name="gameId"/>.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <returns>Returns <see cref="EvaluationResponse"/> that holds Achievement details</returns>
		public EvaluationResponse GetById(string token, int gameId)
		{
			var query = GetUriBuilder("api/achievements/find/{0}/{1}", token, gameId).ToString();
			return Get<EvaluationResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Get all global achievements, ie. achievements that are not associated with a specific game
		/// </summary>
		/// <returns>Returns multiple <see cref="EvaluationResponse"/> that hold Achievement details</returns>
		public IEnumerable<EvaluationResponse> GetAllGlobal()
		{
			var query = GetUriBuilder("api/achievements/global/list").ToString();
			return Get<IEnumerable<EvaluationResponse>>(query);
		}

		/// <summary>
		/// Find a list of Achievements that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">game ID</param>
		/// <returns>Returns multiple <see cref="EvaluationResponse"/> that hold Achievement details</returns>
		public IEnumerable<EvaluationResponse> GetByGame(int gameId)
		{
			var query = GetUriBuilder("api/achievements/game/{0}/list", gameId).ToString();
			return Get<IEnumerable<EvaluationResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all global achievements for <param name="actorId"/>.
		/// </summary>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold Achievement progress details</returns>
		public IEnumerable<EvaluationProgressResponse> GetGlobalProgress(int actorId)
		{
			var query = GetUriBuilder("api/achievements/global/evaluate/{0}", actorId).ToString();
			return Get<IEnumerable<EvaluationProgressResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all achievements for a <param name="gameId"/> for <param name="actorId"/>.
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold current progress toward achievement.</returns>
		public IEnumerable<EvaluationProgressResponse> GetGameProgress(int gameId, int actorId)
		{
			var query = GetUriBuilder("api/achievements/game/{0}/evaluate/{1}", gameId, actorId).ToString();
			return Get<IEnumerable<EvaluationProgressResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for an Achievement for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="actorId">ID of actor/User</param>
		/// <returns>Returns <see cref="EvaluationProgressResponse"/> that hold current progress toward achievement.</returns>
		public EvaluationProgressResponse GetGlobalAchievementProgress(string token, int actorId)
		{
			var query = GetUriBuilder("api/achievements/{0}/global/evaluate/{1}", token, actorId).ToString();
			return Get<EvaluationProgressResponse>(query);
		}

		/// <summary>
		/// Find the current progress for an Achievement for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		/// <param name="actorId">ID of actor/User</param>
		/// <returns>Returns <see cref="EvaluationProgressResponse"/> that hold current progress toward achievement.</returns>
		public EvaluationProgressResponse GetAchievementProgress(string token, int gameId, int actorId)
		{
			var query = GetUriBuilder("api/achievements/{0}/{1}/evaluate/{2}", token, gameId, actorId).ToString();
			return Get<EvaluationProgressResponse>(query);
		}

		/// <summary>
		/// Create a new Achievement.
		/// Requires <see cref="EvaluationRequest.Name"/> to be unique to that <see cref="EvaluationRequest.GameId"/>.
		/// </summary>
		/// <param name="newAchievement"><see cref="EvaluationRequest"/> object that holds the details of the new Achievement.</param>
		/// <returns>Returns a <see cref="EvaluationResponse"/> object containing details for the newly created Achievement.</returns>
		public EvaluationResponse Create(EvaluationCreateRequest newAchievement)
		{
			var query = GetUriBuilder("api/achievements/create").ToString();
			return Post<EvaluationCreateRequest, EvaluationResponse>(query, newAchievement);
		}

		/// <summary>
		/// Update an existing Achievement.
		/// </summary>
		/// <param name="achievement"><see cref="EvaluationRequest"/> object that holds the details of the Achievement.</param>
		public void Update(EvaluationUpdateRequest achievement)
		{
			var query = GetUriBuilder("api/achievements/update").ToString();
			Put(query, achievement);
		}

		/// <summary>
		/// Delete a global achievement, ie. an achievement that is not associated with a specific game
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		public void DeleteGlobal(string token)
		{
			var query = GetUriBuilder("api/achievements/{0}/global", token).ToString();
			Delete(query);
		}

		/// <summary>
		/// Delete Achievement with the <param name="token"/> and <param name="gameId"/> provided.
		/// </summary>
		/// <param name="token">Token of Achievement</param>
		/// <param name="gameId">ID of the Game the Achievement is for</param>
		public void Delete(string token, int gameId)
		{
			var query = GetUriBuilder("api/achievements/{0}/{1}", token, gameId).ToString();
			Delete(query);
		}
	}
}
