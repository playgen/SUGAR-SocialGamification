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
            IList<Contracts.Game> gameContracts = null;
            foreach (DataModel.Game gameModel in gameModels) {
                gameContracts.Add(ToContract(gameModel));
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

        public static Actor ToContract(this User userModel)
        {
            var actorContract = new Actor();
            actorContract.Id = userModel.Id;
            actorContract.Name = userModel.Name;

            return actorContract;
        }
    }
}