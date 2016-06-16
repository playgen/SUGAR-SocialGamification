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

        public static User ToUserModel(this Contracts.Actor actorContract)
        {
            var actorModel = new User();
            actorModel.Id = actorContract.Id;
            actorModel.Name = actorContract.Name;

            return actorModel;
        }

        public static Group ToGroupModel(this Contracts.Actor actorContract)
        {
            var actorModel = new Group();
            actorModel.Id = actorContract.Id;
            actorModel.Name = actorContract.Name;

            return actorModel;
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

        public static UserToUserRelationship ToUserModel(this Contracts.Relationship relationContract)
        {
            var relationModel = new UserToUserRelationship();
            relationModel.RequestorId = relationContract.RequestorId;
            relationModel.AcceptorId = relationContract.AcceptorId;

            return relationModel;
        }

        public static UserToGroupRelationship ToGroupModel(this Contracts.Relationship relationContract)
        {
            var relationModel = new UserToGroupRelationship();
            relationModel.RequestorId = relationContract.RequestorId;
            relationModel.AcceptorId = relationContract.AcceptorId;

            return relationModel;
        }
    }
}
