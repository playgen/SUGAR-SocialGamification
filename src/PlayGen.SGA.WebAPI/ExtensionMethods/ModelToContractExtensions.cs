using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.ExtensionMethods
{
    public static class ModelToContractExtensions
    {
        public static Game ToContract(this DataModel.Game gameModel)
        {
            var gameContract = new Game();
            gameContract.Id = gameModel.Id;
            gameContract.Name = gameModel.Name;

            return gameContract;
        }

        public static Actor ToContract(this DataModel.Group groupModel)
        {
            var actorContract = new Actor();
            actorContract.Id = groupModel.Id;
            actorContract.Name = groupModel.Name;

            return actorContract;
        }

        public static Actor ToContract(this DataModel.User userModel)
        {
            var actorContract = new Actor();
            actorContract.Id = userModel.Id;
            actorContract.Name = userModel.Name;

            return actorContract;
        }
    }
}