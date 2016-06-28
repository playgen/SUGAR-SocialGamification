using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlayGen.SUGAR.Contracts;
using Newtonsoft.Json;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.ExtensionMethods
{
	public static class ContractToModelExtensions
	{
		public static Account ToModel(this AccountRequest accountContract)
		{
			return new Account
			{
				Name = accountContract.Name,
				PasswordHash = accountContract.Password
			};
		}

		public static Game ToModel(this GameRequest gameContract)
		{
			var gameModel = new Game();
			gameModel.Name = gameContract.Name;

			return gameModel;
		}

		public static User ToUserModel(this ActorRequest actorContract)
		{
			var actorModel = new User();
			actorModel.Name = actorContract.Name;

			return actorModel;
		}

		public static Group ToGroupModel(this ActorRequest actorContract)
		{
			var actorModel = new Group();
			actorModel.Name = actorContract.Name;

			return actorModel;
		}

		public static UserAchievement ToUserModel(this AchievementRequest achieveContract)
		{
			var achieveModel = new UserAchievement
			{
				Name = achieveContract.Name,
				GameId = achieveContract.GameId,
				CompletionCriteriaCollection = achieveContract.CompletionCriteria.ToModel(),
				RewardCollection = achieveContract.Reward.ToModel()
			};

			return achieveModel;
		}

		public static GroupAchievement ToGroupModel(this AchievementRequest achieveContract)
		{
			var achieveModel = new GroupAchievement
			{
				Name = achieveContract.Name,
				GameId = achieveContract.GameId,
				CompletionCriteriaCollection = achieveContract.CompletionCriteria.ToModel(),
				RewardCollection = achieveContract.Reward.ToModel()
			};

			return achieveModel;
		}

		public static AchievementCriteriaCollection ToModel(this List<AchievementCriteria> achievementContracts)
		{
			var achievementCollection = new AchievementCriteriaCollection();
			foreach (var achievementContract in achievementContracts)
			{
				achievementCollection.Add(achievementContract.ToModel());
			}

			return achievementCollection;
		}

		public static AchievementCriteria ToModel(this AchievementCriteria achievementContract)
		{
			return new AchievementCriteria
			{
				Key = achievementContract.Key,
				ComparisonType = (ComparisonType) achievementContract.ComparisonType,
				DataType = (GameDataValueType) achievementContract.DataType,
				Value = achievementContract.Value,
			};
		}

		public static RewardCollection ToModel(this List<Reward> rewardContracts)
		{
			var rewardCollection = new RewardCollection();
			foreach (var rewardContract in rewardContracts)
			{
				rewardCollection.Add(rewardContract.ToModel());
			}

			return rewardCollection;
		}

		public static Reward ToModel(this Reward rewardContract)
		{
			return new Reward
			{
				Key = rewardContract.Key,
				DataType = (GameDataValueType)rewardContract.DataType,
				Value = rewardContract.Value,
			};
		}

		public static UserToUserRelationship ToUserModel(this RelationshipRequest relationContract)
		{
			var relationModel = new UserToUserRelationship
			{
				RequestorId = relationContract.RequestorId,
				AcceptorId = relationContract.AcceptorId
			};

			return relationModel;
		}

		public static UserToGroupRelationship ToGroupModel(this RelationshipRequest relationContract)
		{
			var relationModel = new UserToGroupRelationship
			{
				RequestorId = relationContract.RequestorId,
				AcceptorId = relationContract.AcceptorId
			};

			return relationModel;
		}

		public static UserData ToUserModel(this SaveDataRequest dataContract)
		{
			var dataModel = new UserData
			{
				UserId = dataContract.ActorId,
				GameId = dataContract.GameId,
				Key = dataContract.Key,
				Value = dataContract.Value,
				DataType = (GameDataValueType) dataContract.GameDataValueType
			};

			return dataModel;
		}

		public static GroupData ToGroupModel(this SaveDataRequest dataContract)
		{
			var dataModel = new GroupData
			{
				GroupId = dataContract.ActorId,
				GameId = dataContract.GameId,
				Key = dataContract.Key,
				Value = dataContract.Value,
				DataType = (GameDataValueType) dataContract.GameDataValueType
			};

			return dataModel;
		}
	}
}
