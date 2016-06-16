using System.Collections.Generic;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.WebAPI.ExtensionMethods
{
    public static class ModelToContractExtensions
    {
        public static Contracts.Game ToContract(this DataModel.Game gameModel)
        {
            var gameContract = new Contracts.Game();
            gameContract.Id = gameModel.Id;
            gameContract.Name = gameModel.Name;

            return gameContract;
        }

        public static IEnumerable<Contracts.Game> ToContract(this IEnumerable<DataModel.Game> gameModels)
        {
            IList<Contracts.Game> gameContracts = new List<Contracts.Game>();
            foreach (DataModel.Game gameModel in gameModels) {
                var gameContract = ToContract(gameModel);
                gameContracts.Add(gameContract);
            }

            return gameContracts;
        }

        public static Actor ToContract(this Group groupModel)
        {
            var actorContract = new Actor();
            actorContract.Id = groupModel.Id;
            actorContract.Name = groupModel.Name;

            return actorContract;
        }

        public static IEnumerable<Actor> ToContract(this IEnumerable<Group> groupModels)
        {
            IList<Actor> actorContracts = new List<Actor>();
            foreach (Group groupModel in groupModels)
            {
                var actorContract = ToContract(groupModel);
                actorContracts.Add(actorContract);
            }

            return actorContracts;
        }

        public static Actor ToContract(this User userModel)
        {
            var actorContract = new Actor();
            actorContract.Id = userModel.Id;
            actorContract.Name = userModel.Name;

            return actorContract;
        }

        public static IEnumerable<Actor> ToContract(this IEnumerable<User> userModels)
        {
            IList<Actor> actorContracts = new List<Actor>();
            foreach (User userModel in userModels)
            {
                var actorContract = ToContract(userModel);
                actorContracts.Add(actorContract);
            }

            return actorContracts;
        }

        public static Achievement ToContract(this GroupAchievement groupModel)
        {
            var achievementContract = new Achievement();
            achievementContract.Id = groupModel.Id;
            achievementContract.Name = groupModel.Name;
            achievementContract.GameId = groupModel.GameId;
            achievementContract.CompletionCriteria = groupModel.CompletionCriteria;

            return achievementContract;
        }

        public static IEnumerable<Achievement> ToContract(this IEnumerable<GroupAchievement> groupModels)
        {
            IList<Achievement> achievementContracts = new List<Achievement>();
            foreach (GroupAchievement groupModel in groupModels)
            {
                var achievementContract = ToContract(groupModel);
                achievementContracts.Add(achievementContract);
            }

            return achievementContracts;
        }

        public static Achievement ToContract(this UserAchievement userModel)
        {
            var achievementContract = new Achievement();
            achievementContract.Id = userModel.Id;
            achievementContract.Name = userModel.Name;
            achievementContract.GameId = userModel.GameId;
            achievementContract.CompletionCriteria = userModel.CompletionCriteria;

            return achievementContract;
        }

        public static IEnumerable<Achievement> ToContract(this IEnumerable<UserAchievement> userModels)
        {
            IList<Achievement> achievementContracts = new List<Achievement>();
            foreach (UserAchievement userModel in userModels)
            {
                var achievementContract = ToContract(userModel);
                achievementContracts.Add(achievementContract);
            }

            return achievementContracts;
        }

        public static SaveData ToContract(this GroupData groupModel)
        {
            var dataContract = new SaveData();
            dataContract.Id = groupModel.Id;
            dataContract.ActorId = groupModel.GroupId;
            dataContract.GameId = groupModel.GameId;
            dataContract.Key = groupModel.Key;
            dataContract.Value = groupModel.Value;
            dataContract.DataType = (Contracts.DataType)groupModel.DataType;

            return dataContract;
        }

        public static IEnumerable<SaveData> ToContract(this IEnumerable<GroupData> groupModels)
        {
            IList<SaveData> dataContracts = new List<SaveData>();
            foreach (GroupData groupModel in groupModels)
            {
                var dataContract = ToContract(groupModel);
                dataContracts.Add(dataContract);
            }

            return dataContracts;
        }

        public static SaveData ToContract(this UserData groupModel)
        {
            var dataContract = new SaveData();
            dataContract.Id = groupModel.Id;
            dataContract.ActorId = groupModel.UserId;
            dataContract.GameId = groupModel.GameId;
            dataContract.Key = groupModel.Key;
            dataContract.DataType = (Contracts.DataType)groupModel.DataType;

            return dataContract;
        }

        public static IEnumerable<SaveData> ToContract(this IEnumerable<UserData> groupModels)
        {
            IList<SaveData> dataContracts = new List<SaveData>();
            foreach (UserData groupModel in groupModels)
            {
                var dataContract = ToContract(groupModel);
                dataContracts.Add(dataContract);
            }

            return dataContracts;
        }
    }
}