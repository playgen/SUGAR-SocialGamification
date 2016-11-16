using System;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    /// <summary>
    /// Mappings of game data keys to evaluations with criteria that make use of the specific keys.
    /// </summary>
    public class EvaluationGameDataMapper
    {
        // todo create mapping game data key to related evaluation identifier list
        // game data key + game id + actor id => evaluation

        internal IEnumerable<Evaluation> GetRelated(GameData gameData)
        {
            // todo get evaluations that were added to the mapping that evaluate this specific bit of game data
            throw new NotImplementedException();
        }

        internal void CreateMappings(Evaluation evaluation)
        {
            // todo go through criteria and create mappings from the game data to the evaluation
            throw new NotImplementedException();
        }

        internal void RemoveMappings(Evaluation evaluation)
        {
            // todo all occurences of this evaluation from the mappings.
            // note: remember to remove gamedata mappings that don't have any other evaluations mapped any longer
            throw new NotImplementedException();
        }

        internal void MapExisting(List<Evaluation> evaluations)
        {
            // todo remove all mappings
            // todo remap all mappings
            throw new NotImplementedException();
        }
    }
}
