using System.Collections.Concurrent;
using System.Collections.Generic;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Extensions;
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

            ConcurrentDictionary<int, Evaluation> relatedEvalautions;
            if (_mappings.TryGetValue(mappedKey, out relatedEvalautions))
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

                ConcurrentDictionary<int, Evaluation> mappedEvaluationsForKey;

                if (!_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
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

                ConcurrentDictionary<int, Evaluation> mappedEvaluationsForKey;
                if (_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
                {
                    Evaluation removedEvaluation;
                    if (mappedEvaluationsForKey.TryRemove(evaluation.Id, out removedEvaluation) && mappedEvaluationsForKey.Count == 0)
                    {
                        ConcurrentDictionary<int, Evaluation> removedEvaluations;
                        _mappings.TryRemove(mappingKey, out removedEvaluations);
                    }
                }
            }
        }

        private string CreateMappingKey(int? gameId, EvaluationDataType dataType, string EvaluationDataKey)
        {
            return $"{gameId.ToInt()};{dataType};{EvaluationDataKey}";
        }
    }
}
