using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Skill specific operations.
	/// </summary>
	public class SkillClient : ClientBase
	{
		private const string ControllerPrefix = "api/skills";

		public SkillClient(string baseAddress, IHttpHandler httpHandler, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, evaluationNotifications)
		{
		}

		/// <summary>
		/// Find a Global Skill that matches <param name="token"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <returns>Returns <see cref="EvaluationResponse"/> that holds Skill details</returns>
		public EvaluationResponse GetGlobalById(string token)
		{
			var query = GetUriBuilder(ControllerPrefix + "/find/{0}/global", token).ToString();
			return Get<EvaluationResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Find a Skill that matches <param name="token"/> and <param name="gameId"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <returns>Returns <see cref="EvaluationResponse"/> that holds Skill details</returns>
		public EvaluationResponse GetById(string token, int gameId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/find/{0}/{1}", token, gameId).ToString();
			return Get<EvaluationResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Get all global skills, ie. skills that are not associated with a specific game
		/// </summary>
		/// <returns>Returns multiple <see cref="EvaluationResponse"/> that hold Skill details</returns>
		public IEnumerable<EvaluationResponse> GetAllGlobal()
		{
			var query = GetUriBuilder(ControllerPrefix + "/global/list").ToString();
			return Get<IEnumerable<EvaluationResponse>>(query);
		}

		/// <summary>
		/// Find a list of Skills that match <param name="gameId"/>.
		/// </summary>
		/// <param name="gameId">Game ID</param>
		/// <returns>Returns multiple <see cref="EvaluationResponse"/> that hold Skill details</returns>
		public IEnumerable<EvaluationResponse> GetByGame(int gameId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/game/{0}/list", gameId).ToString();
			return Get<IEnumerable<EvaluationResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all global skills for <param name="actorId"/>.
		/// </summary>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold Skill progress details</returns>
		public IEnumerable<EvaluationProgressResponse> GetGlobalProgress(int actorId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/global/evaluate/{0}", actorId).ToString();
			return Get<IEnumerable<EvaluationProgressResponse>>(query);
		}

		/// <summary>
		/// Find the current progress for all skills for a <param name="gameId"/> for <param name="actorId"/>.
		/// </summary>
		/// <param name="gameId">ID of Game</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns multiple <see cref="EvaluationProgressResponse"/> that hold current progress toward skill.</returns>
		public IEnumerable<EvaluationProgressResponse> GetGameProgress(int gameId, int actorId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/game/{0}/evaluate/{1}", gameId, actorId).ToString();
			return Get<IEnumerable<EvaluationProgressResponse>>(query);
		}

		public void GetGameProgressAsync(int gameId, int actorId, Action<IEnumerable<EvaluationProgressResponse>> success, Action<Exception> error)
		{
			try
			{
				var result = GetGameProgress(gameId, actorId);
				success(result);
			}
			catch (Exception e)
			{
				error(e);
			}
		}

		/// <summary>
		/// Find the current progress for an Skill for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="actorId">ID of actor/User</param>
		/// <returns>Returns <see cref="EvaluationProgressResponse"/> that hold current progress toward skill.</returns>
		public EvaluationProgressResponse GetGlobalSkillProgress(string token, int actorId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/global/evaluate/{1}", token, actorId).ToString();
			return Get<EvaluationProgressResponse>(query);
		}

		/// <summary>
		/// Find the current progress for a Skill for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <param name="actorId">ID of Group/User</param>
		/// <returns>Returns <see cref="EvaluationProgressResponse"/> that hold current progress toward skill.</returns>
		public EvaluationProgressResponse GetSkillProgress(string token, int gameId, int actorId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}/evaluate/{2}", token, gameId, actorId).ToString();
			return Get<EvaluationProgressResponse>(query);
		}

		/// <summary>
		/// Create a new Skill.
		/// Requires <see cref="EvaluationCreateRequest.Name"/> to be unique to that <see cref="EvaluationCreateRequest.GameId"/>.
		/// </summary>
		/// <param name="newSkill"><see cref="EvaluationCreateRequest"/> object that holds the details of the new Skill.</param>
		/// <returns>Returns a <see cref="EvaluationResponse"/> object containing details for the newly created Skill.</returns>
		public EvaluationResponse Create(EvaluationCreateRequest newSkill)
		{
			var query = GetUriBuilder(ControllerPrefix + "/create").ToString();
			return Post<EvaluationCreateRequest, EvaluationResponse>(query, newSkill);
		}

		/// <summary>
		/// Update an existing Skill.
		/// </summary>
		/// <param name="skill"><see cref="EvaluationCreateRequest"/> object that holds the details of the Skill.</param>
		public void Update(EvaluationUpdateRequest skill)
		{
			var query = GetUriBuilder(ControllerPrefix + "/update").ToString();
			Put(query, skill);
		}

		/// <summary>
		/// Delete a global skill, ie. a skill that is not associated with a specific game
		/// </summary>
		/// <param name="token">Token of Skill</param>
		public void DeleteGlobal(string token)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/global", token).ToString();
			Delete(query);
		}

		/// <summary>
		/// Delete Skill with the  <param name="token"/> and <param name="gameId"/> provided.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		public void Delete(string token, int gameId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}", token, gameId).ToString();
			Delete(query);
		}

		#region Evaluation Notifications
		/// <summary>
		/// Sets flag to return pending skill notifications from the server as they become available.
		/// </summary>
		/// <param name="enable">Whether to enable or disable notifications.</param>
		public void EnableNotifications(bool enable)
		{
			EnableEvaluationNotifications(enable);
		}

		/// <summary>
		/// Gets pending skill progress notifications.
		/// </summary>
		/// <param name="notification"></param>
		/// <returns>Returns a boolean value indicating whether there was a notification to retrieve or not.</returns>
		public bool TryGetPendingNotification(out EvaluationNotification notification)
		{
			return EvaluationNotifications.TryDequeue(EvaluationType.Skill, out notification);
		}
		#endregion
	}
}
