using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class SkillExtensions
	{
		public static AchievementResponse ToContract(this Skill model)
		{
			var skillContract = new AchievementResponse
			{
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				CompletionCriteria = model.CompletionCriteriaCollection.ToContractList(),
				Reward = model.RewardCollection.ToContractList(),
			};

			return skillContract;
		}

		public static IEnumerable<AchievementResponse> ToContractList(this IEnumerable<Skill> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static Skill ToSkillModel(this AchievementRequest skillContract)
		{
			var skillModel = new Skill
			{
				Name = skillContract.Name,
				Description = skillContract.Description,
				GameId = skillContract.GameId ?? 0,
				ActorType = skillContract.ActorType,
				Token = skillContract.Token,
				CompletionCriteriaCollection = skillContract.CompletionCriteria.ToSkillModel(),
				RewardCollection = skillContract.Reward.ToSkillModel(),
			};

			return skillModel;
		}

		public static AchievementCriteriaCollection ToSkillModel(this List<AchievementCriteria> skillContracts)
		{
			var skillCollection = new AchievementCriteriaCollection();
			if (skillContracts != null)
			{
				skillCollection.Add(skillContracts.Select(ToModel).ToList());
			}

			return skillCollection;
		}

		public static AchievementCriteria ToModel(this Contracts.AchievementCriteria skillContract)
		{
			return new AchievementCriteria
			{
				Key = skillContract.Key,
				ComparisonType = (ComparisonType)skillContract.ComparisonType,
				DataType = (GameDataType)skillContract.DataType,
				Scope = (CriteriaScope)skillContract.Scope,
				Value = skillContract.Value,
			};
		}

		public static RewardCollection ToSkillModel(this List<Reward> skillContracts)
		{
			var rewardCollection = new RewardCollection();
			if (skillContracts != null)
			{
				rewardCollection.Add(skillContracts.Select(ToModel).ToList());
			}

			return rewardCollection;
		}

		public static Reward ToModel(this Contracts.Reward skillContract)
		{
			return new Reward
			{
				Key = skillContract.Key,
				DataType = (GameDataType)skillContract.DataType,
				Value = skillContract.Value,
			};
		}
	}
}