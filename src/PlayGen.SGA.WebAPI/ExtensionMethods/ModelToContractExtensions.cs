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
    }
}