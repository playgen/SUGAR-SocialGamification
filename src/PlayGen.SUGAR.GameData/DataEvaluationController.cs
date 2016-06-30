using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.GameData
{
	public class DataEvaluationController
	{
		protected readonly IGameDataController GameDataController;

		public DataEvaluationController(IGameDataController gameDataController)
		{
			GameDataController = gameDataController;
		}

		// TODO: currently this is binary but should eventually return a progress value
		// The method of returning calculating the progress (for multiple criteria conditions) and 
		// how the progress is going to be represented (0f to 1f ?) need to be determined first.
		public bool IsCriteriaSatisified(int? gameId, int? actorId, AchievementCriteriaCollection completionCriterias)
		{
			return completionCriterias.All(cc => Evaluate(gameId, actorId, cc));
		}


		protected bool Evaluate(int? gameId, int? actorId, AchievementCriteria completionCriteria)
		{
			if (completionCriteria.Scope == CriteriaScope.RelatedActors)
			{
				throw new NotImplementedException("RelatedActors Scope is not implemented");
			}

			switch (completionCriteria.DataType)
			{
				case GameDataType.Boolean:
					return EvaluateBool(gameId, actorId, completionCriteria);

				case GameDataType.String:
					return EvaluateString(gameId, actorId, completionCriteria);

				case GameDataType.Float:
					return EvaluateFloat(gameId, actorId, completionCriteria);

				case GameDataType.Long:
					return EvaluateLong(gameId, actorId, completionCriteria);

				default:
					return false;
			}
		}

		protected bool EvaluateLong(int? gameId, int? actorId, AchievementCriteria completionCriteria)
		{
			var sum = GameDataController.SumLongs(gameId, actorId, completionCriteria.Key);

			return CompareValues(sum, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
		}

		protected bool EvaluateFloat(int? gameId, int? actorId, AchievementCriteria completionCriteria)
		{
			var sum = GameDataController.SumFloats(gameId, actorId, completionCriteria.Key);

			return CompareValues(sum, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
		}

		protected bool EvaluateString(int? gameId, int? actorId, AchievementCriteria completionCriteria)
		{
			string latest;
			return GameDataController.TryGetLatestString(gameId, actorId, completionCriteria.Key, out latest) 
				&& CompareValues(latest, completionCriteria.Value, completionCriteria.ComparisonType);
		}

		protected bool EvaluateBool(int? gameId, int? actorId, AchievementCriteria completionCriteria)
		{
			bool latest;
			if (!GameDataController.TryGetLatestBool(gameId, actorId, completionCriteria.Key, out latest))
			{
				latest = false;
			}

			return CompareValues(latest, bool.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
		}

		protected static bool CompareValues<T>(T value, T expected, ComparisonType comparrisonType) where T : IComparable
		{
			var comparisonResult = value.CompareTo(expected);

			switch (comparrisonType)
			{
				case ComparisonType.Equals:
					return comparisonResult == 0;

				case ComparisonType.NotEqual:
					return comparisonResult != 0;

				case ComparisonType.Greater:
					return comparisonResult > 0;

				case ComparisonType.GreaterOrEqual:
					return comparisonResult == 0 || comparisonResult > 0;

				case ComparisonType.Less:
					return comparisonResult < 0;

				case ComparisonType.LessOrEqual:
					return comparisonResult == 0 || comparisonResult < 0;

				default:
					throw new NotImplementedException($"There is no case for the comparrison type: {comparrisonType}.");
			}
		}
	}
}
