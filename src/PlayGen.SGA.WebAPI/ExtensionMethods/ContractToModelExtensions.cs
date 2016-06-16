using PlayGen.SGA.Contracts;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.WebAPI.ExtensionMethods
{
    public static class ContractToModelExtensions
    {
        public static DataModel.Game ToModel(this Contracts.Game gameContract)
        {
            var gameModel = new DataModel.Game();
            gameModel.Name = gameContract.Name;

            return gameModel;
        }

        public static User ToUserModel(this Actor actorContract)
        {
            var actorModel = new User();
            actorModel.Name = actorContract.Name;

            return actorModel;
        }

        public static Group ToGroupModel(this Actor actorContract)
        {
            var actorModel = new Group();
            actorModel.Name = actorContract.Name;

            return actorModel;
        }

        public static UserAchievement ToUserModel(this Achievement achieveContract)
        {
            var achieveModel = new UserAchievement();
            achieveModel.Name = achieveContract.Name;
            achieveModel.GameId = achieveContract.GameId;
            achieveModel.CompletionCriteria = achieveContract.CompletionCriteria;

            return achieveModel;
        }

        public static GroupAchievement ToGroupModel(this Achievement achieveContract)
        {
            var achieveModel = new GroupAchievement();
            achieveModel.Name = achieveContract.Name;
            achieveModel.GameId = achieveContract.GameId;
            achieveModel.CompletionCriteria = achieveContract.CompletionCriteria;

            return achieveModel;
        }

        public static UserToUserRelationship ToUserModel(this Relationship relationContract)
        {
            var relationModel = new UserToUserRelationship();
            relationModel.RequestorId = relationContract.RequestorId;
            relationModel.AcceptorId = relationContract.AcceptorId;

            return relationModel;
        }

        public static UserToGroupRelationship ToGroupModel(this Relationship relationContract)
        {
            var relationModel = new UserToGroupRelationship();
            relationModel.RequestorId = relationContract.RequestorId;
            relationModel.AcceptorId = relationContract.AcceptorId;

            return relationModel;
        }

        public static UserData ToUserModel(this SaveData dataContract)
        {
            var dataModel = new UserData();
            dataModel.UserId = dataContract.ActorId;
            dataModel.GameId = dataContract.GameId;
            dataModel.Key = dataContract.Key;
            dataModel.Value = dataContract.Value;
            dataModel.DataType = (DataModel.DataType)dataContract.DataType;

            return dataModel;
        }

        public static GroupData ToGroupModel(this SaveData dataContract)
        {
            var dataModel = new GroupData();
            dataModel.GroupId = dataContract.ActorId;
            dataModel.GameId = dataContract.GameId;
            dataModel.Key = dataContract.Key;
            dataModel.Value = dataContract.Value;
            dataModel.DataType = (DataModel.DataType)dataContract.DataType;

            return dataModel;
        }
    }
}
