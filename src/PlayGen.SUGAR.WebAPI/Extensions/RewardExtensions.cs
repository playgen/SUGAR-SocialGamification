using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class RewardExtensions
	{
		public static Reward ToContract(this Reward reward)
		{
			if (reward == null)
			{
				return null;
			}
			return new Reward
			{
				Key = reward.Key,
				DataType = reward.DataType,
				Value = reward.Value,
			};
		}

		public static List<Contracts.Reward> ToContractList(this RewardCollection rewardCollection)
		{
			return rewardCollection.Select(reward => reward.ToContract()).ToList();
		}
	}
}
