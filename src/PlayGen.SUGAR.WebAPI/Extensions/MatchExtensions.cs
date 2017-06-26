using System.Collections.Generic;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;
using System.Linq;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
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
				GameId = contract.GameId,
				MatchId = contract.MatchId,
				ActorId = contract.CreatingActorId,
				Key = contract.Key,
				Value = contract.Value,
				EvaluationDataType = contract.EvaluationDataType,
				Category = EvaluationDataCategory.MatchData,
			};
		}
	}
}
