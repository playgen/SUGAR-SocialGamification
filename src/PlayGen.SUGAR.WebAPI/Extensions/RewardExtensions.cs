using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class RewardExtensions
	{
        public static List<RewardResponse> ToContractList(this List<Reward> rewards)
        {
            return rewards?.Select(ToContract).ToList();
        }

        public static RewardResponse ToContract(this Reward reward)
		{
			if (reward == null)
			{
				return null;
			}
			return new RewardResponse
			{
                Id = reward.Id,
				SaveDataKey = reward.SaveDataKey,
				SaveDataType = reward.SaveDataType,
				Value = reward.Value,
			};
		}

        public static List<Reward> ToModelList(this List<RewardCreateRequest> rewards)
		{
		    return rewards?.Select(ToModel).ToList();
		}

        public static List<Reward> ToModelList(this List<RewardUpdateRequest> rewards)
        {
            return rewards?.Select(ToModel).ToList();
        }

        public static Reward ToModel(this RewardCreateRequest contract)
        {
            return new Reward
            {
                SaveDataKey = contract.SaveDataKey,
                SaveDataType = contract.SaveDataType,
                Value = contract.Value,
            };
        }

        public static Reward ToModel(this RewardUpdateRequest contract)
        {
            return new Reward
            {
                Id = contract.Id,
                SaveDataKey = contract.SaveDataKey,
                SaveDataType = contract.SaveDataType,
                Value = contract.Value,
            };
        }
    }
}
