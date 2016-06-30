using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;

namespace PlayGen.SUGAR.GameData
{
	public class DataEvaluationController
	{
		protected readonly IGameDataController GameDataController;
		protected readonly GroupRelationshipController GroupRelationshipController;

		public DataEvaluationController(IGameDataController gameDataController, GroupRelationshipController groupRelationshipController)
		{
			GameDataController = gameDataController;
			GroupRelationshipController = groupRelationshipController;
		}

		// TODO: currently this is binary but should eventually return a progress value
		// The method of returning calculating the progress (for multiple criteria conditions) and 
		// how the progress is going to be represented (0f to 1f ?) need to be determined first.
		public bool IsCriteriaSatisified(int? gameId, int? actorId, AchievementCriteriaCollection completionCriterias, ActorType actorType)
		{
			return completionCriterias.All(cc => Evaluate(gameId, actorId, cc, actorType));
		}


		protected bool Evaluate(int? gameId, int? actorId, AchievementCriteria completionCriteria, ActorType actorType)
		{
			if (completionCriteria.Scope == CriteriaScope.RelatedActors && actorId != null)
			{
				if (actorType != ActorType.Group)
				{
					throw new NotImplementedException("RelatedActors Scope is only implemented for groups");
				}
				var groupActors = GroupRelationshipController.GetMembers(actorId.Value);
				switch (completionCriteria.DataType)
				{
					case GameDataType.Boolean:
						return EvaluateManyBool(gameId, groupActors, completionCriteria);

					case GameDataType.String:
						return EvaluateManyString(gameId, groupActors, completionCriteria);

					case GameDataType.Float:
						return EvaluateManyFloat(gameId, groupActors, completionCriteria);

					case GameDataType.Long:
						return EvaluateManyLong(gameId, groupActors, completionCriteria);

					default:
						return false;
				}
			}
			else
			{
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

		protected bool EvaluateManyLong(int? gameId, IEnumerable<Actor> actor, AchievementCriteria completionCriteria)
		{
			var sum = actor.Sum(a => GameDataController.SumLongs(gameId, a.Id, completionCriteria.Key));

			return CompareValues(sum, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
		}

		protected bool EvaluateManyFloat(int? gameId, IEnumerable<Actor> actor, AchievementCriteria completionCriteria)
		{
			var sum = actor.Sum(a => GameDataController.SumFloats(gameId, a.Id, completionCriteria.Key));

			return CompareValues(sum, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType);
		}

		protected bool EvaluateManyString(int? gameId, IEnumerable<Actor> actor, AchievementCriteria completionCriteria)
		{
			return actor.All(a => EvaluateString(gameId, a.Id, completionCriteria) == true);
		}

		protected bool EvaluateManyBool(int? gameId, IEnumerable<Actor> actor, AchievementCriteria completionCriteria)
		{
			return actor.All(a => EvaluateBool(gameId, a.Id, completionCriteria) == true);
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
