using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.Development
{
	/// <summary>
	/// Controller that facilitates developer specific operations.
	/// </summary>
	public class DevelopmentClient : ClientBase
	{
		private const string ControllerPrefix = "api/";

		public DevelopmentClient(
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
		/// Create a new Game.
		/// </summary>
		/// <param name="game"><see cref="GameRequest"/> object that holds the details of the new game request.</param>
		/// <returns>A <see cref="GameResponse"/> containing the new game details.</returns>
		public GameResponse CreateGame(GameRequest game)
		{
			var query = GetUriBuilder(ControllerPrefix + "game").ToString();
			return Post<GameRequest, GameResponse>(query, game);
		}

		public void CreateGameAsync(GameRequest game, Action<GameResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => CreateGame(game),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Create a new Achievement.
		/// </summary>
		/// <param name="game"><see cref="EvaluationCreateRequest"/> object that holds the details of the new achievement request.</param>
		/// <returns>A <see cref="GameResponse"/> containing the new achievement details.</returns>
		public EvaluationResponse CreateAchievement(EvaluationCreateRequest achievement)
		{
			var query = GetUriBuilder(ControllerPrefix + "achievements/create").ToString();
			return Post<EvaluationCreateRequest, EvaluationResponse>(query, achievement);
		}

		public void CreateAchievementAsync(EvaluationCreateRequest achievement, Action<EvaluationResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => CreateAchievement(achievement),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Create a new Skill.
		/// </summary>
		/// <param name="game"><see cref="EvaluationCreateRequest"/> object that holds the details of the new skill request.</param>
		/// <returns>A <see cref="GameResponse"/> containing the new skill details.</returns>
		public EvaluationResponse CreateSkill(EvaluationCreateRequest skill)
		{
			var query = GetUriBuilder(ControllerPrefix + "skills/create").ToString();
			return Post<EvaluationCreateRequest, EvaluationResponse>(query, skill);
		}

		public void CreateSkillAsync(EvaluationCreateRequest skill, Action<EvaluationResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => CreateSkill(skill),
				onSuccess,
				onError);
		}

		/// <summary>
		/// Create a new Leaderboard.
		/// </summary>
		/// <param name="game"><see cref="EvaluationCreateRequest"/> object that holds the details of the new leaderboard request.</param>
		/// <returns>A <see cref="GameResponse"/> containing the new leaderboard details.</returns>
		public LeaderboardResponse CreateLeaderboard(LeaderboardRequest leaderboard)
		{
			var query = GetUriBuilder(ControllerPrefix + "leaderboards/create").ToString();
			return Post<LeaderboardRequest, LeaderboardResponse>(query, leaderboard);
		}

		public void CreateLeaderboardAsync(LeaderboardRequest leaderboard, Action<LeaderboardResponse> onSuccess, Action<Exception> onError)
		{
			AsyncRequestController.EnqueueRequest(() => CreateLeaderboard(leaderboard),
				onSuccess,
				onError);
		}
	}
}