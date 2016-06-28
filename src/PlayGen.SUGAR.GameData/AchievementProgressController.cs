using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.GameData
{
	public class AchievementProgressController
	{
		private readonly IGameDataController _gameDataController;

		public AchievementProgressController(IGameDataController gameDataController)
		{
			_gameDataController = gameDataController;
		}

		// TODO: currently this is binary but should eveltually return a progress value
		// The method of returning calculating the progress (for multiple criteria conditions) and 
		// how the progress is going to be represented (0f to 1f ?) need to be determined first.
		public bool IsAchievementSatisified(int gameId, int userId, AchievementCriteriaCollection completionCriterias)
		{
			return completionCriterias.All(cc => Evaluate(gameId, userId, cc));
		}

		public bool IsAchievementCompleted(int gameId, int achievementId, int userId)
		{
			string key = $"GameId{gameId}AchievementId{achievementId}";
			bool check;
			if (!_gameDataController.TryGetLatestBool(gameId, userId, key, out check))
			{
				return false;
			}

			return CompareValues<bool>(check, true, ComparisonType.Equals);
		}

		private bool Evaluate(int gameId, int userId, AchievementCriteria completionCriteria)
		{
			bool passed = false;

			switch (completionCriteria.DataType)
			{
				case GameDataValueType.Boolean:
					passed = EvaluateBool(gameId, userId, completionCriteria);
					break;

				case GameDataValueType.String:
					passed = EvaluateString(gameId, userId, completionCriteria);
					break;

				case GameDataValueType.Float:
					passed = EvaluateFloat(gameId, userId, completionCriteria);
					break;

				case GameDataValueType.Long:
					passed = EvaluateLong(gameId, userId, completionCriteria);
					break;
			}

			return passed;
		}

		private bool EvaluateLong(int gameId, int userId, AchievementCriteria completionCriteria)
		{
			long sum = _gameDataController.SumLongs(gameId, userId, completionCriteria.Key);

			return CompareValues<long>(sum, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
		}

		private bool EvaluateFloat(int gameId, int userId, AchievementCriteria completionCriteria)
		{
			float sum = _gameDataController.SumFloats(gameId, userId, completionCriteria.Key);

			return CompareValues<float>(sum, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
		}

		private bool EvaluateString(int gameId, int userId, AchievementCriteria completionCriteria)
		{
			string latest;
			if (!_gameDataController.TryGetLatestString(gameId, userId, completionCriteria.Key, out latest))
			{
				return false;
			}

			return CompareValues<string>(latest, completionCriteria.Value, completionCriteria.ComparisonType);
		}

		private bool EvaluateBool(int gameId, int userId, AchievementCriteria completionCriteria)
		{
			bool latest;
			if (!_gameDataController.TryGetLatestBool(gameId, userId, completionCriteria.Key, out latest))
			{
				latest = false;
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
