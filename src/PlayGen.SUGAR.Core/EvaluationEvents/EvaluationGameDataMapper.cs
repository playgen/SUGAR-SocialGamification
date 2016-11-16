using System;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Mappings of game data keys to evaluations with criteria that make use of the specific keys.
    /// </summary>
    public class EvaluationGameDataMapper
    {
        private readonly Dictionary<string, HashSet<Evaluation>> _mappings = new Dictionary<string, HashSet<Evaluation>>();

        public bool TryGetRelated(GameData gameData, out HashSet<Evaluation> relatedEvaluations)
        {
            var mappedKey = CreateMappingKey(gameData.GameId, gameData.DataType, gameData.Key);
            return _mappings.TryGetValue(mappedKey, out relatedEvaluations);
        }

        public void CreateMappings(Evaluation evaluation)
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

        public void RemoveMappings(Evaluation evaluation)
        {
            // todo all occurences of this evaluation from the mappings.
            // note: remember to remove gamedata mappings that don't have any other evaluations mapped any longer
            throw new NotImplementedException();
        }

        public void MapExisting(List<Evaluation> evaluations)
        {
            // todo remove all mappings
            // todo remap all mappings
            throw new NotImplementedException();
        }

        private string CreateMappingKey(int? gameId, GameDataType dataType, string gameDataKey)
        {
            return $"{gameId};{dataType};{gameDataKey}";
        }
    }
}
