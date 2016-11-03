using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class RewardExtensions
	{
		public static Contracts.Shared.Reward ToContract(this Common.Shared.Reward reward)
		{
			if (reward == null)
			{
				return null;
			}
			return new Contracts.Shared.Reward
			{
				Key = reward.Key,
				DataType = reward.DataType,
				Value = reward.Value,
			};
		}

		public static List<Contracts.Shared.Reward> ToContractList(this List<Common.Shared.Reward> rewards)
		{
			return rewards.Select(ToContract).ToList();
		}

		public static Common.Shared.Reward ToModel(this Contracts.Shared.Reward reward)
		{
			if (reward == null)
			{
				return null;
			}
			return new Common.Shared.Reward
			{
				Key = reward.Key,
				DataType = reward.DataType,
				Value = reward.Value,
			};
		}

		public static List<Common.Shared.Reward> ToModelList(this List<Contracts.Shared.Reward> rewards)
		{
			return rewards.Select(ToModel).ToList();
		}
	}
}
