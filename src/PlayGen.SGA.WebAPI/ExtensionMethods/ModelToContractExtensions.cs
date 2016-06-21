using System.Collections.Generic;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.DataModel;
using Newtonsoft.Json;

namespace PlayGen.SGA.WebAPI.ExtensionMethods
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
            var gameContract = new GameResponse();
            gameContract.Id = gameModel.Id;
            gameContract.Name = gameModel.Name;

            return gameContract;
        }

        public static IEnumerable<GameResponse> ToContract(this IEnumerable<Game> gameModels)
        {
            IList<GameResponse> gameContracts = new List<GameResponse>();
            foreach (Game gameModel in gameModels) {
                var gameContract = ToContract(gameModel);
                gameContracts.Add(gameContract);
            }

            return gameContracts;
        }

        public static ActorResponse ToContract(this Group groupModel)
        {
            var actorContract = new ActorResponse();
            actorContract.Id = groupModel.Id;
            actorContract.Name = groupModel.Name;

            return actorContract;
        }

        public static IEnumerable<ActorResponse> ToContract(this IEnumerable<Group> groupModels)
        {
            IList<ActorResponse> actorContracts = new List<ActorResponse>();
            foreach (Group groupModel in groupModels)
            {
                var actorContract = ToContract(groupModel);
                actorContracts.Add(actorContract);
            }

            return actorContracts;
        }

        public static ActorResponse ToContract(this User userModel)
        {
            var actorContract = new ActorResponse();
            actorContract.Id = userModel.Id;
            actorContract.Name = userModel.Name;

            return actorContract;
        }

        public static IEnumerable<ActorResponse> ToContract(this IEnumerable<User> userModels)
        {
            IList<ActorResponse> actorContracts = new List<ActorResponse>();
            foreach (User userModel in userModels)
            {
                var actorContract = ToContract(userModel);
                actorContracts.Add(actorContract);
            }

            return actorContracts;
        }

        public static AchievementResponse ToContract(this GroupAchievement groupModel)
        {
            var achievementContract = new AchievementResponse();
            achievementContract.Id = groupModel.Id;
            achievementContract.Name = groupModel.Name;
            achievementContract.GameId = groupModel.GameId;
            achievementContract.CompletionCriteria = JsonConvert.DeserializeObject<List<AchievementCriteria>>(groupModel.CompletionCriteria);

            return achievementContract;
        }

        public static IEnumerable<AchievementResponse> ToContract(this IEnumerable<GroupAchievement> groupModels)
        {
            IList<AchievementResponse> achievementContracts = new List<AchievementResponse>();
            foreach (GroupAchievement groupModel in groupModels)
            {
                var achievementContract = ToContract(groupModel);
                achievementContracts.Add(achievementContract);
            }

            return achievementContracts;
        }

        public static AchievementResponse ToContract(this UserAchievement userModel)
        {
            var achievementContract = new AchievementResponse();
            achievementContract.Id = userModel.Id;
            achievementContract.Name = userModel.Name;
            achievementContract.GameId = userModel.GameId;
            achievementContract.CompletionCriteria = userModel.CompletionCriteriaCollection.ToContract();

            return achievementContract;
        }

        // TODO test - again can probably do with linq
        public static List<AchievementCriteria> ToContract(this CompletionCriteriaCollection completionCriteriaCollection)
        {
            var achievementCriterias = new List<AchievementCriteria>();
            foreach (var completionCriteria in completionCriteriaCollection)
            {
                achievementCriterias.Add(completionCriteria.ToContract());
            }

            return achievementCriterias;
        }

        // TODO test
        public static AchievementCriteria ToContract(this CompletionCriteria completionCriteria)
        {
            return new AchievementCriteria
            {
                Key = completionCriteria.Key,
                DataType = (Contracts.DataType) completionCriteria.DataType,
                ComparisonType = (Contracts.ComparisonType) completionCriteria.ComparisonType,
                Value = completionCriteria.Value,
            };
        }

        public static IEnumerable<AchievementResponse> ToContract(this IEnumerable<UserAchievement> userModels)
        {
            IList<AchievementResponse> achievementContracts = new List<AchievementResponse>();
            foreach (UserAchievement userModel in userModels)
            {
                var achievementContract = ToContract(userModel);
                achievementContracts.Add(achievementContract);
            }

            return achievementContracts;
        }

        public static SaveDataResponse ToContract(this GroupData groupModel)
        {
            var dataContract = new SaveDataResponse();
            dataContract.ActorId = groupModel.GroupId;
            dataContract.GameId = groupModel.GameId;
            dataContract.Key = groupModel.Key;
            dataContract.Value = groupModel.Value;
            dataContract.DataType = (Contracts.DataType)groupModel.DataType;

            return dataContract;
        }

        public static IEnumerable<SaveDataResponse> ToContract(this IEnumerable<GroupData> groupModels)
        {
            IList<SaveDataResponse> dataContracts = new List<SaveDataResponse>();
            foreach (GroupData groupModel in groupModels)
            {
                var dataContract = ToContract(groupModel);
                dataContracts.Add(dataContract);
            }

            return dataContracts;
        }

        public static SaveDataResponse ToContract(this UserData groupModel)
        {
            var dataContract = new SaveDataResponse();
            dataContract.ActorId = groupModel.UserId;
            dataContract.GameId = groupModel.GameId;
            dataContract.Key = groupModel.Key;
            dataContract.Value = groupModel.Value;
            dataContract.DataType = (Contracts.DataType)groupModel.DataType;

            return dataContract;
        }

        public static IEnumerable<SaveDataResponse> ToContract(this IEnumerable<UserData> groupModels)
        {
            IList<SaveDataResponse> dataContracts = new List<SaveDataResponse>();
            foreach (UserData groupModel in groupModels)
            {
                var dataContract = ToContract(groupModel);
                dataContracts.Add(dataContract);
            }

            return dataContracts;
        }

        public static RelationshipResponse ToContract(this UserToGroupRelationshipRequest relationshipModel)
        {
            var relationshipContract = new RelationshipResponse();
            relationshipContract.RequestorId = relationshipModel.RequestorId;
            relationshipContract.AcceptorId = relationshipModel.AcceptorId;

            return relationshipContract;
        }

        public static RelationshipResponse ToContract(this UserToUserRelationshipRequest relationshipModel)
        {
            var relationshipContract = new RelationshipResponse();
            relationshipContract.RequestorId = relationshipModel.RequestorId;
            relationshipContract.AcceptorId = relationshipModel.AcceptorId;

            return relationshipContract;
        }
    }
}