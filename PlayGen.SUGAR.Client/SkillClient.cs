using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Skill specific operations.
	/// </summary>
	public class SkillClient : ClientBase
	{
		private const string ControllerPrefix = "api/skills";

		public SkillClient(
			string baseAddress,
			IHttpHandler httpHandler,
			Dictionary<string, string> constantHeaders,
			Dictionary<string, string> sessionHeaders,
			IAsyncRequestController asyncRequestController,
			EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, constantHeaders, sessionHeaders, asyncRequestController, evaluationNotifications)
		{
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

		public void GetGlobalProgressAsync(int actorId, Action<IEnumerable<EvaluationProgressResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetGlobalProgress(actorId),
				onSuccess,
				onError);
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

		public void GetGameProgressAsync(int gameId, int actorId, Action<IEnumerable<EvaluationProgressResponse>> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetGameProgress(gameId, actorId),
				onSuccess,
				onError);
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

		public void GetGlobalSkillProgressAsync(string token, int actorId, Action<EvaluationProgressResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetGlobalSkillProgress(token, actorId),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Find the current progress for an Skill for <param name="actorId"/>.
		/// </summary>
		/// <param name="token">Token of Skill</param>
		/// <param name="gameId">ID of the Game the Skill is for</param>
		/// <param name="actorId">ID of actor/User</param>
		/// <returns>Returns <see cref="EvaluationProgressResponse"/> that hold current progress toward skill.</returns>
		public EvaluationProgressResponse GetSkillProgress(string token, int gameId, int actorId)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}/{1}/evaluate/{2}", token, gameId, actorId).ToString();
			return Get<EvaluationProgressResponse>(query);
		}

		public void GetSkillProgressAsync(string token, int gameId, int actorId, Action<EvaluationProgressResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => GetSkillProgress(token, gameId, actorId),
				onSuccess,
				onError);
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
