using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class GameDataExtensions
	{
		public static EvaluationData ToGameDataModel(this EvaluationDataRequest contract)
		{
			return new EvaluationData
			{
				GameId = contract.GameId,
				MatchId = contract.MatchId,
				ActorId = contract.CreatingActorId,
				Key = contract.Key,
				Value = contract.Value,
				EvaluationDataType = contract.EvaluationDataType,
				Category = EvaluationDataCategory.GameData
			};
		}
	}
}