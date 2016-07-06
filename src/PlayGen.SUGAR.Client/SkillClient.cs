using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;


namespace PlayGen.SUGAR.Client
{
	public class SkillClient : ClientBase
	{
		public SkillClient(string baseAddress, Credentials credentials) : base(baseAddress, credentials)
		{
		}

		/// <summary>
		/// Get all global skills, ie. skills that are not associated with a specific game
		/// </summary>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Skill details</returns>
		public IEnumerable<AchievementResponse> Get()
		{
			var query = GetUriBuilder("api/skills/list").ToString();
			return Get<IEnumerable<AchievementResponse>>(query);
		}

		/// <summary>
		/// Find a list of Skills that match <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/skills/game/1/list
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="AchievementResponse"/> that hold Skill details</returns>
		public IEnumerable<AchievementResponse> Get(string gameId)
		{
			var query = GetUriBuilder($"api/skills/game/{gameId}/list").ToString();
			return Get<IEnumerable<AchievementResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for an <param name="skillId"/> for <param name="actorId"/>.
		/// </summary>
		/// <param name="skillId">ID of Skill</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward skill.</returns>
		public IEnumerable<AchievementProgressResponse> GetAchievementProgress(string skillId, string actorId)
		{
			var query = GetUriBuilder($"api/skills/{skillId}/evaluate/{actorId}").ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all skills for a <param name="gameId"/> for <param name="actorId"/>.
		/// 
		/// Example Usage: GET api/skills/game/1/evaluate/1
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward skill.</returns>
		public IEnumerable<AchievementProgressResponse> GetGameProgress(string actorId, string gameId)
		{
			var query = GetUriBuilder($"api/skills/game/{gameId}/evaluate/{actorId}").ToString();
			return Get<IEnumerable<AchievementProgressResponse>>(query);
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
		/// Delete Skills with the <param name="id"/> provided.
		/// </summary>
		/// <param name="id">Skill ID</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder($"api/skills/{id}").ToString();
			Delete(query);
		}

	}
}
