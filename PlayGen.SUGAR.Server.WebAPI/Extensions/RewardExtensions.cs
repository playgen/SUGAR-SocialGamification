using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
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
			return new RewardResponse {
				Id = reward.Id,
				EvaluationDataKey = reward.EvaluationDataKey,
				EvaluationDataType = reward.EvaluationDataType,
				Value = reward.Value
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
			return new Reward {
				EvaluationDataKey = contract.EvaluationDataKey,
				EvaluationDataType = contract.EvaluationDataType,
				EvaluationDataCategory = contract.EvaluationDataCategory,
				Value = contract.Value,
			};
		}

		public static Reward ToModel(this RewardUpdateRequest contract)
		{
			return new Reward {
				Id = contract.Id.Value,
				EvaluationDataKey = contract.EvaluationDataKey,
				EvaluationDataType = contract.EvaluationDataType,
				EvaluationDataCategory = contract.EvaluationDataCategory,
				Value = contract.Value,
			};
		}
	}
}
