using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;
using EvaluationCriteria = PlayGen.SUGAR.Server.Model.EvaluationCriteria;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	internal static class EvaluationExtensions
	{
		public static EvaluationResponse ToContract(this Evaluation model)
		{
			if (model == null)
			{
				return null;
			}

			return new EvaluationResponse {
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				EvaluationCriterias = model.EvaluationCriterias.ToContractList(),
				Rewards = model.Rewards.ToContractList()
			};
		}

		public static IEnumerable<EvaluationResponse> ToContractList(this IEnumerable<Evaluation> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static EvaluationCriteria ToModel(this EvaluationCriteriaCreateRequest contract)
		{
			return new EvaluationCriteria {
				EvaluationDataKey = contract.EvaluationDataKey,
				ComparisonType = contract.ComparisonType,
				CriteriaQueryType = contract.CriteriaQueryType,
				EvaluationDataType = contract.EvaluationDataType,
				Scope = contract.Scope,
				Value = contract.Value
			};
		}

		public static EvaluationCriteria ToModel(this EvaluationCriteriaUpdateRequest contract)
		{
			return new EvaluationCriteria {
				Id = contract.Id.Value,
				EvaluationDataKey = contract.EvaluationDataKey,
				ComparisonType = contract.ComparisonType,
				CriteriaQueryType = contract.CriteriaQueryType,
				EvaluationDataType = contract.EvaluationDataType,
				Scope = contract.Scope,
				Value = contract.Value
			};
		}

		public static IEnumerable<EvaluationProgressResponse> ToContractList(this IEnumerable<EvaluationProgress> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static EvaluationProgressResponse ToContract(this EvaluationProgress model)
		{
			return new EvaluationProgressResponse {
				Actor = model.Actor.ToActorContract(),
				Name = model.Name,
				Progress = model.Progress
			};
		}

		public static List<EvaluationProgressResponse> ToContractList(this ConcurrentDictionary<int, ConcurrentDictionary<Evaluation, float>> pendingNotifications)
		{
			if (pendingNotifications == null)
				return null;

			var progressResponses = new List<EvaluationProgressResponse>();

			foreach (var actorProgress in pendingNotifications)
			{
				foreach (var evaluationProgress in actorProgress.Value)
				{
					progressResponses.Add(new EvaluationProgressResponse {
						Actor = new ActorResponse {
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