using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class ActorDetailsExtensions
	{
		/// <summary>
		/// Convertys a EF Model ActorDetails object to the corresponding DTO
		/// </summary>
		/// <param name="actorDetails"></param>
		/// <returns></returns>
		public static ActorDetailsResponse ToContract(this Data.Model.ActorDetails actorDetails)
		{
			if (actorDetails == null)
			{
				return null;
			}

			return new ActorDetailsResponse
			{
				ActorId = actorDetails.ActorId,
				Key = actorDetails.Key,
				Value = actorDetails.Value,
				EvaluationDataType = actorDetails.EvaluationDataType
			};
		}

		public static IEnumerable<ActorDetailsResponse> ToContractList(this IEnumerable<Data.Model.ActorDetails> actorDatas)
		{
			return actorDatas.Select(ToContract).ToList();
		}

		public static Data.Model.ActorDetails ToActorDataModel(this EvaluationDataRequest dataContract)
		{
			return new Data.Model.ActorDetails
			{
				ActorId = dataContract.ActorId,
				Key = dataContract.Key,
				Value = dataContract.Value,
				EvaluationDataType = dataContract.EvaluationDataType
			};
		}
	}
}