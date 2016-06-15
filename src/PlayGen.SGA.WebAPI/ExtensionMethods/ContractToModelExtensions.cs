using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.WebAPI.ExtensionMethods
{
    public static class ContractToModelExtensions
    {
        public static Game ToModel(this Contracts.Game gameContract)
        {
            var gameModel = new Game();
            gameModel.Id = gameContract.Id;
            gameModel.Name = gameContract.Name;

            return gameModel;
        }

        public static UserAchievement ToUserModel(this Contracts.Achievement achieveContract)
        {
            var achieveModel = new UserAchievement();
            achieveModel.Id = achieveContract.Id;
            achieveModel.Name = achieveContract.Name;
            achieveModel.GameId = achieveContract.GameId;
            achieveModel.CompletionCriteria = achieveContract.CompletionCriteria;

            return achieveModel;
        }

        public static GroupAchievement ToGroupModel(this Contracts.Achievement achieveContract)
        {
            var achieveModel = new GroupAchievement();
            achieveModel.Id = achieveContract.Id;
            achieveModel.Name = achieveContract.Name;
            achieveModel.GameId = achieveContract.GameId;
            achieveModel.CompletionCriteria = achieveContract.CompletionCriteria;

            return achieveModel;
        }
    }
}
