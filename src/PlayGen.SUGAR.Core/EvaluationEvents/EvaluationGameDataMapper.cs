using System;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using System.Linq;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Mappings of game data keys to evaluations with criteria that make use of the specific keys.
    /// </summary>
    public class EvaluationGameDataMapper
    {
        // <gamedata key, <Evaluations>>
        private readonly Dictionary<string, HashSet<Evaluation>> _mappings = new Dictionary<string, HashSet<Evaluation>>();

        public bool TryGetRelated(GameData gameData, out HashSet<Evaluation> relatedEvaluations)
        {
            var mappedKey = CreateMappingKey(gameData.GameId ?? -1, gameData.DataType, gameData.Key);
            return _mappings.TryGetValue(mappedKey, out relatedEvaluations);
        }

        public void CreateMappings(IEnumerable<Evaluation> evaluations)
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
                var mappingKey = CreateMappingKey(evaluation.GameId, evaluationCriteria.DataType, evaluationCriteria.Key);

                HashSet<Evaluation> mappedEvaluationsForKey;

                if (_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
                {
                    mappedEvaluationsForKey.Add(evaluation);
                }
                else
                {
                    _mappings[mappingKey] = new HashSet<Evaluation>
                    {
                        evaluation
                    };
                }
            }
        }

        public void RemoveMapping(Evaluation evaluation)
        {
            foreach (var evaluationCriteria in evaluation.EvaluationCriterias)
            {
                var mappingKey = CreateMappingKey(evaluation.GameId, evaluationCriteria.DataType, evaluationCriteria.Key);

                HashSet<Evaluation> mappedEvaluationsForKey;

                if (_mappings.TryGetValue(mappingKey, out mappedEvaluationsForKey))
                {
                    mappedEvaluationsForKey.Remove(evaluation);

                    if (!mappedEvaluationsForKey.Any())
                    {
                        _mappings.Remove(mappingKey);
                    }
                }
            }
        }

        private string CreateMappingKey(int gameId, GameDataType dataType, string gameDataKey)
        {
            return $"{gameId};{dataType};{gameDataKey}";
        }
    }
}
