using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using PlayGen.SGA.DataController.Interfaces;
using PlayGen.SGA.DataModel;

namespace PlayGen.SGA.AchievementProgress
{
    public class AchievementProgressController
    {
        private readonly ISaveDataDbController _saveDataDbController;

        public AchievementProgressController(ISaveDataDbController saveDataDbController)
        {
            _saveDataDbController = saveDataDbController;
        }

        // TODO: currently this is binary but should eveltually return a progress value
        // The method of returning calculating the progress (for multiple criteria conditions) and 
        // how the progress is going to be represented (0f to 1f ?) need to be determined first.
        public bool GetProgress(int gameId, int userId, CompletionCriteriaCollection completionCriterias)
        {
            // Todo using switch cases for now

            bool passedAll = true;

            foreach (var completionCriteria in completionCriterias)
            {
                passedAll &= Evaluate(gameId, userId, completionCriteria);

                if (!passedAll)
                {
                    break;
                }
            }
            
            return passedAll;
        }

        private bool Evaluate(int gameId, int userId, CompletionCriteria completionCriteria)
        {
            bool passed = false;

            switch (completionCriteria.DataType)
            {
                case DataType.Boolean:
                    passed = EvaluateBool(gameId, userId, completionCriteria);
                    break;

                case DataType.String:
                    passed = EvaluateString(gameId, userId, completionCriteria);
                    break;

                case DataType.Float:
                    passed = EvaluateFloat(gameId, userId, completionCriteria);
                    break;

                case DataType.Long:
                    passed = EvaluateLong(gameId, userId, completionCriteria);
                    break;
            }

            return passed;
        }

        private bool EvaluateLong(int gameId, int userId, CompletionCriteria completionCriteria)
        {
            long sum = _saveDataDbController.SumLongs(gameId, userId, completionCriteria.Key);

            return CompareValues<long>(sum, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
        }

        private bool EvaluateFloat(int gameId, int userId, CompletionCriteria completionCriteria)
        {
            float sum = _saveDataDbController.SumLongs(gameId, userId, completionCriteria.Key);

            return CompareValues<float>(sum, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
        }

        private bool EvaluateString(int gameId, int userId, CompletionCriteria completionCriteria)
        {
            string latest;
            if (!_saveDataDbController.TryGetLatestString(gameId, userId, completionCriteria.Key, out latest))
            {
                return false;
            }

            return CompareValues<string>(latest, completionCriteria.Value, completionCriteria.ComparisonType);
        }

        private bool EvaluateBool(int gameId, int userId, CompletionCriteria completionCriteria)
        {
            bool latest;
            if (!_saveDataDbController.TryGetLatestBool(gameId, userId, completionCriteria.Key, out latest))
            {
                return false;
            }

            return CompareValues<bool>(latest, bool.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
        }

        bool CompareValues<T>(T value, T expected, ComparisonType comparrisonType) where T : IComparable
        {
            int comparrisonResult = value.CompareTo(expected);
            bool passed = false;

            switch (comparrisonType)
            {
                case ComparisonType.Equals:
                    passed = comparrisonResult == 0;
                    break;

                case ComparisonType.NotEqual:
                    passed = comparrisonResult != 0;
                    break;

                case ComparisonType.Greater:
                    passed = comparrisonResult > 0;
                    break;

                case ComparisonType.GreaterOrEqual:
                    passed = comparrisonResult == 0 || comparrisonResult > 0;
                    break;

                case ComparisonType.Less:
                    passed = comparrisonResult < 0;
                    break;

                case ComparisonType.LessOrEqual:
                    passed = comparrisonResult == 0 || comparrisonResult < 0;
                    break;

                default:
                    throw new NotImplementedException($"There is no case for the comparrison type: {comparrisonType}.");
            }

            return passed;
        }

    }
}
