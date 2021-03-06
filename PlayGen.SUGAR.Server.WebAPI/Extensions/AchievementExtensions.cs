﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class AchievementExtensions
	{
		public static EvaluationResponse ToContract(this Achievement model)
		{
			if (model == null)
			{
				return null;
			}

			return new EvaluationResponse {
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				EvaluationCriterias = model.EvaluationCriterias.ToContractList(),
				Rewards = model.Rewards.ToContractList()
			};
		}

		public static IEnumerable<EvaluationResponse> ToContractList(this IEnumerable<Achievement> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static Achievement ToAchievementModel(this EvaluationCreateRequest achievementContract)
		{
			var achievementModel = new Achievement {
				Name = achievementContract.Name,
				Description = achievementContract.Description,
				GameId = achievementContract.GameId.Value,
				ActorType = achievementContract.ActorType.Value,
				Token = achievementContract.Token,
				EvaluationCriterias = achievementContract.EvaluationCriterias.ToModelList(),
				Rewards = achievementContract.Rewards.ToModelList()
			};

			return achievementModel;
		}

		public static Achievement ToAchievementModel(this EvaluationUpdateRequest achievementContract)
		{
			var achievementModel = new Achievement {
				Id = achievementContract.Id.Value,
				Name = achievementContract.Name,
				Description = achievementContract.Description,
				GameId = achievementContract.GameId.Value,
				ActorType = achievementContract.ActorType.Value,
				Token = achievementContract.Token,
				EvaluationCriterias = achievementContract.EvaluationCriterias.ToModelList(),
				Rewards = achievementContract.Rewards.ToModelList()
			};

			return achievementModel;
		}
	}
}