using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class MatchExtensions
	{
		public static MatchesResponse ToCollectionContract(this List<Match> models)
		{
			return new MatchesResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static MatchResponse ToContract(this Match model)
		{
			return new MatchResponse
			{
				Id = model.Id,
				Game = model.Game?.ToContract(),
				Creator = model.Creator?.ToContract(),
				Started = model.Started,
				Ended = model.Ended
			};
		}

		public static EvaluationData ToMatchDataModel(this EvaluationDataRequest contract)
		{
			return new EvaluationData
			{
				GameId = contract.GameId,
				MatchId = contract.MatchId,
				ActorId = contract.CreatingActorId,
				Key = contract.Key,
				Value = contract.Value,
				EvaluationDataType = contract.EvaluationDataType,
				Category = EvaluationDataCategory.MatchData
			};
		}
	}
}