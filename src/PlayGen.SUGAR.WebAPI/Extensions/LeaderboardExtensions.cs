using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class LeaderboardExtensions
	{
		public static LeaderboardResponse ToContract(this Leaderboard leaderboardModel)
		{
			if (leaderboardModel == null)
			{
				return null;
			}


            return new LeaderboardResponse
			{
				GameId = leaderboardModel.GameId,
				Name = leaderboardModel.Name,
				Token = leaderboardModel.Token,
				Key = leaderboardModel.SaveDataKey,
				ActorType = leaderboardModel.ActorType,
				SaveDataType = leaderboardModel.SaveDataType,
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
			return new Leaderboard
			{
				GameId = leaderboardContract.GameId ?? 0,
				Name = leaderboardContract.Name,
				Token = leaderboardContract.Token,
				SaveDataKey = leaderboardContract.Key,
				ActorType = leaderboardContract.ActorType,
				SaveDataType = leaderboardContract.SaveDataType,
				CriteriaScope = leaderboardContract.CriteriaScope,
				LeaderboardType = leaderboardContract.LeaderboardType
			};
		}
	}
}
