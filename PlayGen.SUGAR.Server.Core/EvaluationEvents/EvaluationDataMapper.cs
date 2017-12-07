using System.Collections.Concurrent;
using System.Collections.Generic;

using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
	/// <summary>
	/// Mappings of game data keys to evaluations with criteria that make use of the specific keys.
	/// </summary>
	public class EvaluationDataMapper
	{
		// <EvaluationData key, <evaluationId, evaluation>>
		private readonly ConcurrentDictionary<string, ConcurrentDictionary<int, Evaluation>> _mappings = new ConcurrentDictionary<string, ConcurrentDictionary<int, Evaluation>>();

		public bool TryGetRelated(EvaluationData evaluationData, out ICollection<Evaluation> evaluations)
		{
			var didGetRelated = false;
			evaluations = null;
			var mappedKey = CreateMappingKey(evaluationData.GameId, evaluationData.EvaluationDataType, evaluationData.Key);

			if (_mappings.TryGetValue(mappedKey, out var relatedEvalautions))
			{
				evaluations = relatedEvalautions.Values;
				didGetRelated = true;
			}

			return didGetRelated;
		}

		public void CreateMappings(List<Evaluation> evaluations)
		{
			foreach (var evaluation in evaluations)
			{
				CreateMapping(evaluation);
			}   
		}

		public void CreateMapping(Evaluation evaluation)
		{
			foreach (var evaluationCriteria in evaluation.EvaluationCriterias)
			{
				var mappingKey = CreateMappingKey(evaluation.GameId, evaluationCriteria.EvaluationDataType, evaluationCriteria.EvaluationDataKey);

				if (!_mappings.TryGetValue(mappingKey, out var mappedEvaluationsForKey))
				{
					mappedEvaluationsForKey = new ConcurrentDictionary<int, Evaluation>();
					_mappings[mappingKey] = mappedEvaluationsForKey;
				}

				mappedEvaluationsForKey[evaluation.Id] = evaluation;
			}
		}

		public void RemoveMapping(Evaluation evaluation)
		{
			foreach (var evaluationCriteria in evaluation.EvaluationCriterias)
			{
				var mappingKey = CreateMappingKey(evaluation.GameId, evaluationCriteria.EvaluationDataType, evaluationCriteria.EvaluationDataKey);

				if (_mappings.TryGetValue(mappingKey, out var mappedEvaluationsForKey))
				{
					if (mappedEvaluationsForKey.TryRemove(evaluation.Id, out var _) && mappedEvaluationsForKey.Count == 0)
					{
						_mappings.TryRemove(mappingKey, out var _);
					}
				}
			}
		}

		private string CreateMappingKey(int gameId, EvaluationDataType dataType, string evaluationDataKey)
		{
			return $"{gameId};{dataType};{evaluationDataKey}";
		}
	}
}
