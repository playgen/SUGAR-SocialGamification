using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;
using EvaluationCriteria = PlayGen.SUGAR.Data.Model.EvaluationCriteria;

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

	    public static IEnumerable<EvaluationProgressResponse> ToContractList(this IEnumerable<EvaluationProgress> models)
	    {
	        return models.Select(ToContract).ToList();
	    }

        public static EvaluationProgressResponse ToContract(this EvaluationProgress model)
        {
            return new EvaluationProgressResponse
            {
                Actor = model.Actor.ToContract(),
                Name = model.Name,
                Progress = model.Progress,
            };
        }

        public static List<EvaluationProgressResponse> ToContractList(this ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> pendingNotifications)
        {
            if (pendingNotifications == null) return null;

            var progressResponses = new List<EvaluationProgressResponse>();

            foreach (var actorProgress in pendingNotifications)
            {
                foreach (var evaluationProgress in actorProgress.Value)
                {
                    progressResponses.Add(new EvaluationProgressResponse
                    {
                        Actor = new ActorResponse
                            {
                                // todo get name
                                Id = actorProgress.Key
                            },

                        Name = evaluationProgress.Key.Name,

                        Progress = evaluationProgress.Value,

                        Type = evaluationProgress.Key.EvaluationType
                    });
                }
            }

            return progressResponses;
	    }
    }
}