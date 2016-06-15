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
    }
}