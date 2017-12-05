using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class MatchExtensions
	{
		public static List<MatchResponse> ToContractList(this List<Match> models)
		{
			return models?.Select(ToContract).ToList();
		}

		public static MatchResponse ToContract(this Match model)
		{
			return new MatchResponse {
				Id = model.Id,
				Game = model.Game?.ToContract(),
				Creator = model.Creator?.ToContract(),
				Started = model.Started,
				Ended = model.Ended
			};
		}

		public static EvaluationData ToMatchDataModel(this EvaluationDataRequest contract)
		{
			return new EvaluationData {
				GameId = contract.GameId.Value,
				MatchId = contract.MatchId,
				ActorId = contract.CreatingActorId.Value,
				Key = contract.Key,
				Value = contract.Value,
				EvaluationDataType = contract.EvaluationDataType.Value,
				Category = EvaluationDataCategory.MatchData
			};
		}
	}
}
