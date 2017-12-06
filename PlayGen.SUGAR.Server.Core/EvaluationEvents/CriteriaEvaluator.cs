using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.EntityFramework;
using EvaluationCriteria = PlayGen.SUGAR.Server.Model.EvaluationCriteria;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
	/// <summary>
	/// Evaluates evaluation criteria.
	/// </summary>
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public class CriteriaEvaluator
	{
		protected readonly GroupMemberController GroupMemberCoreController;
		protected readonly UserFriendController UserFriendCoreController;
		protected readonly SUGARContextFactory ContextFactory;

		protected ILogger<EvaluationDataController> EvaluationDataLogger;

		public CriteriaEvaluator(
			ILogger<EvaluationDataController> evaluationDataLogger,
			SUGARContextFactory contextFactory, 
			GroupMemberController groupMemberCoreController, 
			UserFriendController userFriendCoreController)
		{
			EvaluationDataLogger = evaluationDataLogger;
			ContextFactory = contextFactory;
			GroupMemberCoreController = groupMemberCoreController;
			UserFriendCoreController = userFriendCoreController;
		}

		// TODO: currently this is binary but should eventually return a progress value
		// The method of returning calculating the progress (for multiple criteria conditions) and 
		// how the progress is going to be represented (0f to 1f ?) need to be determined first.
		public float IsCriteriaSatisified(int gameId, int actorId, List<EvaluationCriteria> completionCriterias, ActorType actorType, EvaluationType evaluationType)
		{
			return completionCriterias.Sum(cc => Evaluate(gameId, actorId, cc, actorType, evaluationType)) / completionCriterias.Count;
		}

		protected float Evaluate(int gameId, int actorId, EvaluationCriteria completionCriteria, ActorType actorType, EvaluationType evaluationType)
		{ 
			if (completionCriteria.Scope == CriteriaScope.RelatedActors)
			{
				if (actorType != ActorType.Group)
				{
					throw new NotImplementedException("RelatedActors Scope is only implemented for groups");
				}
				var groupActors = GroupMemberCoreController.GetMembers(actorId).ToList<Actor>();
				switch (completionCriteria.EvaluationDataType)
				{
					case EvaluationDataType.Boolean:
						return EvaluateManyBool(gameId, groupActors, completionCriteria);

					case EvaluationDataType.String:
						return EvaluateManyString(gameId, groupActors, completionCriteria);

					case EvaluationDataType.Float:
						return EvaluateManyFloat(gameId, groupActors, completionCriteria);

					case EvaluationDataType.Long:
						return EvaluateManyLong(gameId, groupActors, completionCriteria);

					default:
						return 0;
				}
			}
			switch (completionCriteria.EvaluationDataType)
			{
				case EvaluationDataType.Boolean:
					return EvaluateBool(gameId, actorId, completionCriteria);

				case EvaluationDataType.String:
					return EvaluateString(gameId, actorId, completionCriteria);

				case EvaluationDataType.Float:
					return EvaluateFloat(gameId, actorId, completionCriteria);

				case EvaluationDataType.Long:
					return EvaluateLong(gameId, actorId, completionCriteria);

				default:
					return 0;
			}
		}

		protected float EvaluateLong(int gameId, int actorId, EvaluationCriteria completionCriteria)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, completionCriteria.EvaluationDataCategory);

			switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.List(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Long).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(long.Parse(value.Value), long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Sum:
					if (!evaluationDataController.TryGetSum<long>(gameId, actorId, completionCriteria.EvaluationDataKey, out var sum, EvaluationDataType.Long))
					{
						return 0;
					}
					
					return CompareValues(sum.Value, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					if (!evaluationDataController.TryGetLatest(gameId, actorId, completionCriteria.EvaluationDataKey, out var latest, EvaluationDataType.Long))
					{
						return 0;
					}

					return CompareValues(long.Parse(latest.Value), long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateFloat(int gameId, int actorId, EvaluationCriteria completionCriteria)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, completionCriteria.EvaluationDataCategory);

			switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.List(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Float).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(float.Parse(value.Value), float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Sum:
					if (!evaluationDataController.TryGetSum<float>(gameId, actorId, completionCriteria.EvaluationDataKey, out var sum, EvaluationDataType.Float))
					{
						return 0;
					}

					return CompareValues(sum.Value, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					if (!evaluationDataController.TryGetLatest(gameId, actorId, completionCriteria.EvaluationDataKey, out var latest, EvaluationDataType.Float))
					{
						return 0;
					}

					return CompareValues(float.Parse(latest.Value), float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateString(int gameId, int actorId, EvaluationCriteria completionCriteria)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, completionCriteria.EvaluationDataCategory);

			switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.List(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.String).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(value.Value, completionCriteria.Value, completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Latest:
					if (!evaluationDataController.TryGetLatest(gameId, actorId, completionCriteria.EvaluationDataKey, out var latest, EvaluationDataType.String))
					{
						return 0;
					}

					return CompareValues(latest.Value, completionCriteria.Value, completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateBool(int gameId, int actorId, EvaluationCriteria completionCriteria)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, completionCriteria.EvaluationDataCategory);

			switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.List(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Boolean).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(bool.Parse(value.Value), bool.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Latest:
					if (!evaluationDataController.TryGetLatest(gameId, actorId, completionCriteria.EvaluationDataKey, out var latest, EvaluationDataType.Boolean))
					{
						return 0;
					}

					return CompareValues(bool.Parse(latest.Value), bool.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateManyLong(int gameId, List<Actor> actors, EvaluationCriteria completionCriteria)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, completionCriteria.EvaluationDataCategory);

			switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					return actors.Sum(a => EvaluateLong(gameId, a.Id, completionCriteria)) / actors.Count;
				case CriteriaQueryType.Sum:
					var totalSum = actors.Sum(a => evaluationDataController.TryGetSum<long>(gameId, a.Id, completionCriteria.EvaluationDataKey, out var sum, EvaluationDataType.Long) ? sum.Value : 0);

					return CompareValues(totalSum, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					return actors.Sum(a => EvaluateLong(gameId, a.Id, completionCriteria)) / actors.Count;
				default:
					return 0;
			}
		}

		protected float EvaluateManyFloat(int gameId, List<Actor> actors, EvaluationCriteria completionCriteria)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, completionCriteria.EvaluationDataCategory);
			
			switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					return actors.Sum(a => EvaluateFloat(gameId, a.Id, completionCriteria)) / actors.Count;
				case CriteriaQueryType.Sum:
					var totalSum = actors.Sum(a => evaluationDataController.TryGetSum<float>(gameId, a.Id, completionCriteria.EvaluationDataKey, out var sum, EvaluationDataType.Float) ? sum.Value : 0f);

					return CompareValues(totalSum, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					return actors.Sum(a => EvaluateFloat(gameId, a.Id, completionCriteria)) / actors.Count;
				default:
					return 0;
			}
		}

		protected float EvaluateManyString(int gameId, List<Actor> actors, EvaluationCriteria completionCriteria)
		{
			return actors.Sum(a => EvaluateString(gameId, a.Id, completionCriteria)) / actors.Count;
		}

		protected float EvaluateManyBool(int gameId, List<Actor> actors, EvaluationCriteria completionCriteria)
		{
			return actors.Sum(a => EvaluateBool(gameId, a.Id, completionCriteria)) / actors.Count;
		}

		protected static float CompareValues<T>(T value, T expected, ComparisonType comparisonType, EvaluationDataType dataType) where T : IComparable
		{
			var comparisonResult = value.CompareTo(expected);

			float expectedNum;
			switch (comparisonType)
			{
				case ComparisonType.Equals:
					return comparisonResult == 0 ? 1 : 0;

				case ComparisonType.NotEqual:
					return comparisonResult != 0 ? 1 : 0;

				case ComparisonType.Greater:
					if (comparisonResult > 0)
					{
						return 1;
					}
					if (!(comparisonResult > 0) && (dataType == EvaluationDataType.String || dataType == EvaluationDataType.Boolean))
					{
						return 0;
					}
					if ((float.TryParse(expected.ToString(), out expectedNum))) {
						if (dataType == EvaluationDataType.Long)
						{
							expectedNum += 1;
						} else
						{
							expectedNum += 0.000001f;
						}
						return (float.Parse(value.ToString()) / expectedNum);
					}
					return 0;

				case ComparisonType.GreaterOrEqual:
					if (comparisonResult >= 0)
					{
						return 1;
					}
					if (!(comparisonResult >= 0) && (dataType == EvaluationDataType.String || dataType == EvaluationDataType.Boolean))
					{
						return 0;
					}
					if ((float.TryParse(expected.ToString(), out expectedNum)))
					{
						return (float.Parse(value.ToString()) / expectedNum);
					}
					return 0;

				case ComparisonType.Less:
					return comparisonResult < 0 ? 1 : 0;

				case ComparisonType.LessOrEqual:
					return comparisonResult <= 0 ? 1 : 0;

				default:
					throw new NotImplementedException($"There is no case for the comparison type: {comparisonType}.");
			}
		}
	}
}
