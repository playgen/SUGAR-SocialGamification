﻿using NLog;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;
using System;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class RewardController : CriteriaEvaluator
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		public RewardController(SUGARContextFactory contextFactory,
			GroupMemberController groupMemberCoreController,
			UserFriendController userFriendCoreController)
			: base(contextFactory, groupMemberCoreController, userFriendCoreController)
		{
		}

		public bool AddReward(int? actorId, int? gameId, Reward reward)
		{
			var evaluationDataController = new EvaluationDataController(ContextFactory, reward.EvaluationDataCategory);

			var evaluationData = new EvaluationData {
				Key = reward.EvaluationDataKey,
				GameId = gameId,    //TODO: handle the case where a global achievement has been completed for a specific game
				ActorId = actorId,
				Category = reward.EvaluationDataCategory,
				EvaluationDataType = reward.EvaluationDataType,
				Value = reward.Value
			};

			evaluationDataController.Add(evaluationData);

			Logger.Info($"Game Data: {evaluationData?.Id} for ActorId: {actorId}, GameId: {gameId}, Reward: {reward?.Id}");

			return true;
		}
	}
}