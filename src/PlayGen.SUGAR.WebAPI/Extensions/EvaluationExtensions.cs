using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	internal static class EvaluationExtensions
	{
        public static EvaluationResponse ToContract(this Evaluation model)
		{
			if (model == null)
			{
				return null;
			}

            return new EvaluationResponse
			{
                Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				EvaluationCriterias = model.EvaluationCriterias.ToContractList(),
				Rewards = model.Rewards.ToContractList(),
			};
		}

        public static IEnumerable<EvaluationResponse> ToContractList(this IEnumerable<Evaluation> models)
		{
			return models.Select(ToContract).ToList();
		}

        public static EvaluationCriteria ToModel(this EvaluationCriteriaCreateRequest contract)
		{
			return new EvaluationCriteria
            {
				Key = contract.Key,
				ComparisonType = contract.ComparisonType,
				CriteriaQueryType = contract.CriteriaQueryType,
				DataType = contract.DataType,
				Scope = contract.Scope,
				Value = contract.Value,
			};
		}

        public static EvaluationCriteria ToModel(this EvaluationCriteriaUpdateRequest contract)
        {
            return new EvaluationCriteria
            {
                Id = contract.Id,
                Key = contract.Key,
                ComparisonType = contract.ComparisonType,
                CriteriaQueryType = contract.CriteriaQueryType,
                DataType = contract.DataType,
                Scope = contract.Scope,
                Value = contract.Value,
            };
        }
    }
}