using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.ExtensionMethods
{
	public static class ModelToContractExtensions
	{
		public static AccountResponse ToContract(this Account accountModel)
		{
			return new AccountResponse
			{
				User = accountModel.User.ToContract()
			};
		}

		public static GameResponse ToContract(this Game gameModel)
		{
			var gameContract = new GameResponse
			{
				Id = gameModel.Id,
				Name = gameModel.Name
			};

			return gameContract;
		}

		public static IEnumerable<GameResponse> ToContract(this IEnumerable<Game> gameModels)
		{
			return gameModels.Select(ToContract).ToList();
		}

		public static ActorResponse ToContract(this Group groupModel)
		{
			var actorContract = new ActorResponse
			{
				Id = groupModel.Id,
				Name = groupModel.Name
			};

			return actorContract;
		}

		public static IEnumerable<ActorResponse> ToContract(this IEnumerable<Group> groupModels)
		{
			return groupModels.Select(ToContract).ToList();
		}

		public static ActorResponse ToContract(this User userModel)
		{
			var actorContract = new ActorResponse
			{
				Id = userModel.Id,
				Name = userModel.Name
			};

			return actorContract;
		}

		public static IEnumerable<ActorResponse> ToContract(this IEnumerable<User> userModels)
		{
			return userModels.Select(ToContract).ToList();
		}

		public static AchievementResponse ToContract(this GroupAchievement groupModel)
		{
			var achievementContract = new AchievementResponse
			{
				Id = groupModel.Id,
				Name = groupModel.Name,
				GameId = groupModel.GameId,
				CompletionCriteria = groupModel.CompletionCriteriaCollection.ToContract()
			};

			return achievementContract;
		}

		public static IEnumerable<AchievementResponse> ToContract(this IEnumerable<GroupAchievement> groupModels)
		{
			return groupModels.Select(ToContract).ToList();
		}

		public static AchievementResponse ToContract(this UserAchievement userModel)
		{
			var achievementContract = new AchievementResponse
			{
				Id = userModel.Id,
				Name = userModel.Name,
				GameId = userModel.GameId,
				CompletionCriteria = userModel.CompletionCriteriaCollection.ToContract()
			};

			return achievementContract;
		}

		public static IEnumerable<AchievementResponse> ToContract(this IEnumerable<UserAchievement> userModels)
		{
			return userModels.Select(ToContract).ToList();
		}

		public static AchievementCriteria ToContract(this AchievementCriteria completionCriteria)
		{
			return new AchievementCriteria
			{
				Key = completionCriteria.Key,
				DataType = completionCriteria.DataType,
				ComparisonType = completionCriteria.ComparisonType,
				Value = completionCriteria.Value,
			};
		}

		public static List<Contracts.AchievementCriteria> ToContract(this AchievementCriteriaCollection completionCriteriaCollection)
		{
			return completionCriteriaCollection.Select(completionCriteria => completionCriteria.ToContract()).ToList();
		}

		public static SaveDataResponse ToContract(this GroupData groupModel)
		{
			var dataContract = new SaveDataResponse
			{
				ActorId = groupModel.GroupId,
				GameId = groupModel.GameId,
				Key = groupModel.Key,
				Value = groupModel.Value,
				GameDataValueType = groupModel.DataType
			};

			return dataContract;
		}

		public static IEnumerable<SaveDataResponse> ToContract(this IEnumerable<GroupData> groupModels)
		{
			return groupModels.Select(ToContract).ToList();
		}

		public static SaveDataResponse ToContract(this UserData groupModel)
		{
			var dataContract = new SaveDataResponse
			{
				ActorId = groupModel.UserId,
				GameId = groupModel.GameId,
				Key = groupModel.Key,
				Value = groupModel.Value,
				GameDataValueType = groupModel.DataType
			};

			return dataContract;
		}

		public static IEnumerable<SaveDataResponse> ToContract(this IEnumerable<UserData> groupModels)
		{
			return groupModels.Select(ToContract).ToList();
		}

		public static RelationshipResponse ToContract(this UserToGroupRelationship relationshipModel)
		{
			var relationshipContract = new RelationshipResponse
			{
				RequestorId = relationshipModel.RequestorId,
				AcceptorId = relationshipModel.AcceptorId
			};

			return relationshipContract;
		}

		public static RelationshipResponse ToContract(this UserToUserRelationship relationshipModel)
		{
			var relationshipContract = new RelationshipResponse
			{
				RequestorId = relationshipModel.RequestorId,
				AcceptorId = relationshipModel.AcceptorId
			};

			return relationshipContract;
		}
	}
}