using System.Diagnostics.CodeAnalysis;

using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class GameDataExtensions
	{
		public static EvaluationData ToGameDataModel(this EvaluationDataRequest contract)
		{
			return new EvaluationData {
				GameId = contract.GameId.Value,
				MatchId = contract.MatchId,
				ActorId = contract.CreatingActorId.Value,
				Key = contract.Key,
				Value = contract.Value,
				EvaluationDataType = contract.EvaluationDataType.Value,
				Category = EvaluationDataCategory.GameData
			};
		}
	}
}
