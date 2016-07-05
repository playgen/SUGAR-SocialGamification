using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class LeaderboardExtensions
	{
		public static LeaderboardResponse ToContract(this Leaderboard leaderboardModel)
		{
			var leaderboardContract = new LeaderboardResponse
			{
				Id = leaderboardModel.Id,
				GameId = leaderboardModel.GameId,
				Name = leaderboardModel.Name,
				Token = leaderboardModel.Token,
				Key = leaderboardModel.Key,
				ActorType = leaderboardModel.ActorType,
				GameDataType = leaderboardModel.GameDataType,
				CriteriaScope = leaderboardModel.CriteriaScope,
				LeaderboardType = leaderboardModel.LeaderboardType
			};

			return leaderboardContract;
		}

		public static IEnumerable<LeaderboardResponse> ToContractList(this IEnumerable<Leaderboard> leaderboardModels)
		{
			return leaderboardModels.Select(ToContract).ToList();
		}

		public static Leaderboard ToModel(this LeaderboardRequest leaderboardContract)
		{
			var leaderboardModel = new Leaderboard
			{
				GameId = leaderboardContract.GameId,
				Name = leaderboardContract.Name,
				Token = leaderboardContract.Token,
				Key = leaderboardContract.Key,
				ActorType = leaderboardContract.ActorType,
				GameDataType = leaderboardContract.GameDataType,
				CriteriaScope = leaderboardContract.CriteriaScope,
				LeaderboardType = leaderboardContract.LeaderboardType
			};

			return leaderboardModel;
		}
	}
}
