using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Skill specific operations.
	/// </summary>
	public class SkillClient : ClientBase
	{
		public SkillClient(string baseAddress, Credentials credentials, IHttpHandler httpHandler)
			: base(baseAddress, credentials, httpHandler)
		{
		}

		/// <summary>
		/// Find a Global Skill that matches <param name="token"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <returns>Returns <see cref="AchievementResponse"/> that holds Skill details</returns>
		public AchievementResponse GetGlobalById(string token)
		{
			var query = GetUriBuilder("api/skills/find/{0}/global", token).ToString();
			return Get<AchievementResponse>(query, new System.Net.HttpStatusCode[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Find a Skill that matches <param name="token"/> and <param name="gameId"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <returns>Returns <see cref="AchievementResponse"/> that holds Skill details</returns>
		public AchievementResponse GetById(string token, int gameId)
		{
			var query = GetUriBuilder("api/skills/find/{0}/{1}", token, gameId).ToString();
			return Get<AchievementResponse>(query, new System.Net.HttpStatusCode[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Get all global skills, ie. skills that are not associated with a specific game
		/// </summary>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Skill details</returns>
		public IEnumerable<AchievementResponse> GetAllGlobal()
		{
			var query = GetUriBuilder("api/skills/global/list").ToString();
			return Get<IEnumerable<AchievementResponse>>(query);
		}

		/// <summary>
		/// Find a list of Skills that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Skill details</returns>
		public IEnumerable<AchievementResponse> GetByGame(int gameId)
		{
			var query = GetUriBuilder("api/skills/game/{0}/list", gameId).ToString();
			return Get<IEnumerable<AchievementResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all global skills for <param name="actorId"/>.
		/// </summary>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold Skill progress details</returns>
		public IEnumerable<AchievementProgressResponse> GetGlobalProgress(int actorId)
		{
			var query = GetUriBuilder("api/skills/global/evaluate/{0}", actorId).ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all skills for a <param name="gameId"/> for <param name="actorId"/>.
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current progress toward skill.</returns>
		public IEnumerable<AchievementProgressResponse> GetGameProgress(int gameId, int actorId)
		{
			var query = GetUriBuilder("api/skills/game/{0}/evaluate/{1}", gameId, actorId).ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for an Skill for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="actorId">ID of actor/User</param>
		/// <returns>Returns <see cref="AchievementProgressResponse"/> that hold current progress toward skill.</returns>
		public AchievementProgressResponse GetGlobalSkillProgress(string token, int actorId)
		{
			var query = GetUriBuilder("api/skills/{0}/global/evaluate/{1}", token, actorId).ToString();
			return Get<AchievementProgressResponse>(query);
		}

		/// <summary>
		/// Find the current progress for a Skill for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns <see cref="AchievementProgressResponse"/> that hold current progress toward skill.</returns>
		public AchievementProgressResponse GetSkillProgress(string token, int gameId, int actorId)
		{
			var query = GetUriBuilder("api/skills/{0}/{1}/evaluate/{2}", token, gameId, actorId).ToString();
			return Get<AchievementProgressResponse>(query);
		}

		/// <summary>
		/// Create a new Skill.
		/// Requires <see cref="AchievementRequest.Name"/> to be unique to that <see cref="AchievementRequest.GameId"/>.
		/// </summary>
		/// <param name="newSkill"><see cref="AchievementRequest"/> object that holds the details of the new Skill.</param>
		/// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created Skill.</returns>
		public AchievementResponse Create(AchievementRequest newSkill)
		{
			var query = GetUriBuilder("api/skills/create").ToString();
			return Post<AchievementRequest, AchievementResponse>(query, newSkill);
		}

		/// <summary>
		/// Update an existing Skill.
		/// </summary>
		/// <param name="skill"><see cref="AchievementRequest"/> object that holds the details of the Skill.</param>
		public void Update(AchievementRequest skill)
		{
			var query = GetUriBuilder("api/skills/update").ToString();
			Put(query, skill);
		}

		/// <summary>
		/// Delete a global skill, ie. a skill that is not associated with a specific game
		/// </summary>
		/// <param name="token">Token of Skill</param>
		public void DeleteGlobal(string token)
		{
			var query = GetUriBuilder("api/skills/{0}/global", token).ToString();
			Delete(query);
		}

		/// <summary>
		/// Delete Skill with the  <param name="token"/> and <param name="gameId"/> provided.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		public void Delete(string token, int gameId)
		{
			var query = GetUriBuilder("api/skills/{0}/{1}", token, gameId).ToString();
			Delete(query);
		}

	}
}
