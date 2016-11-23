using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
    public class ProgressCache
    {
        // <gameId, <actorId, <evaluation, progress>>>
        private readonly Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> _progressMappings = new Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>>();

        public bool TryGetValue(int? gameId, out Dictionary<int, Dictionary<Evaluation, float>> gameProgress)
        {
            return _progressMappings.TryGetValue(gameId.ToInt(), out gameProgress);
        }

        // <actorId, <evaluation, progress>>
        public Dictionary<int, Dictionary<Evaluation, float>> this[int? index]
        {
            get { return _progressMappings[index.ToInt()]; }
            set { _progressMappings[index.ToInt()] = value; }
        }
    }
}
