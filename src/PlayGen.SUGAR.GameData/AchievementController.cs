using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.GameData
{
	public class AchievementController : DataEvaluationController
	{
		public AchievementController(IGameDataController gameDataController)
			: base(gameDataController)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="achievement"></param>
		/// <param name="actorId"></param>
		/// <returns></returns>
		public bool IsAchievementCompleted(Achievement achievement, int? actorId)
		{
			

			var key = string.Format(KeyConstants.AchievementCompleteFormat, achievement.Token);
			var completed =  GameDataController.KeyExists(achievement.GameId, actorId, key);

			if (!completed)
			{
				completed = IsCriteriaSatisified(achievement.GameId, actorId, achievement.CompletionCriteriaCollection);
				if (completed)
				{
					ProcessAchievementRewards(achievement, actorId);
				}
			}

			return completed;
		}

		private void ProcessAchievementRewards(Achievement achievement, int? actorId)
		{
			var gameData = new Data.Model.GameData()
			{
				Key = string.Format(KeyConstants.AchievementCompleteFormat, achievement.Token),
				GameId = achievement.GameId,    //TODO: handle the case where a global achievement has been completed for a specific game
				ActorId = actorId,
				DataType = GameDataType.String,
				Value = null
			};
			GameDataController.Create(gameData);
		}

		public void EvaluateAchievement(Achievement achievement, int? actorId)
		{
		}
	}
}