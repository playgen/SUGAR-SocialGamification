using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GameDataExtensions
	{
		public static Data.Model.EvaluationData ToGameDataModel(this EvaluationDataRequest contract)
		{
			return new Data.Model.EvaluationData {
				GameId = contract.GameId,
				MatchId = contract.MatchId,
				ActorId = contract.CreatingActorId,
				Key = contract.Key,
				Value = contract.Value,
				EvaluationDataType = contract.EvaluationDataType,
				Category = EvaluationDataCategory.GameData,
			};
		}
	}
}
