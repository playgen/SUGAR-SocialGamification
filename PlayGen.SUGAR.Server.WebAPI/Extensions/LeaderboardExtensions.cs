using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public static class LeaderboardExtensions
	{
		public static LeaderboardResponse ToContract(this Leaderboard leaderboardModel)
		{
			if (leaderboardModel == null)
			{
				return null;
			}

			return new LeaderboardResponse {
				GameId = leaderboardModel.GameId,
				Name = leaderboardModel.Name,
				Token = leaderboardModel.Token,
				EvaluationDataCategory = leaderboardModel.EvaluationDataCategory,
				Key = leaderboardModel.EvaluationDataKey,
				ActorType = leaderboardModel.ActorType,
				EvaluationDataType = leaderboardModel.EvaluationDataType,
				CriteriaScope = leaderboardModel.CriteriaScope,
				LeaderboardType = leaderboardModel.LeaderboardType
			};
		}

		public static IEnumerable<LeaderboardResponse> ToContractList(this IEnumerable<Leaderboard> leaderboardModels)
		{
			return leaderboardModels.Select(ToContract).ToList();
		}

		public static Leaderboard ToModel(this LeaderboardRequest leaderboardContract)
		{
			return new Leaderboard {
				GameId = leaderboardContract.GameId.Value,
				Name = leaderboardContract.Name,
				Token = leaderboardContract.Token,
				EvaluationDataCategory = leaderboardContract.EvaluationDataCategory.Value,
				EvaluationDataKey = leaderboardContract.Key,
				ActorType = leaderboardContract.ActorType.Value,
				EvaluationDataType = leaderboardContract.EvaluationDataType.Value,
				CriteriaScope = leaderboardContract.CriteriaScope.Value,
				LeaderboardType = leaderboardContract.LeaderboardType.Value
			};
		}

		public static StandingsRequest ToCore(this LeaderboardStandingsRequest standingsContract)
		{
			return new StandingsRequest
			{
				LeaderboardToken = standingsContract.LeaderboardToken,
				GameId = standingsContract.GameId.Value,
				ActorId = standingsContract.ActorId,
				LeaderboardFilterType = standingsContract.LeaderboardFilterType.Value,
				PageLimit = standingsContract.PageLimit.Value,
				PageOffset = standingsContract.PageOffset.Value,
				MultiplePerActor = standingsContract.MultiplePerActor.Value,
				DateStart = standingsContract.DateStart,
				DateEnd = standingsContract.DateEnd
			};
		}

		public static LeaderboardStandingsResponse ToContract(this StandingsResponse standingsCore)
		{
			return new LeaderboardStandingsResponse
			{
				ActorId = standingsCore.ActorId,
				ActorName = standingsCore.ActorName,
				Value = standingsCore.Value,
				Ranking = standingsCore.Ranking
			};
		}

		public static IEnumerable<LeaderboardStandingsResponse> ToContractList(this IEnumerable<StandingsResponse> standingsCores)
		{
			return standingsCores.Select(ToContract).ToList();
		}
	}
}
