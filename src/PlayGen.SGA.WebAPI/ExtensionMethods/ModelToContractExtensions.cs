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
    }
}