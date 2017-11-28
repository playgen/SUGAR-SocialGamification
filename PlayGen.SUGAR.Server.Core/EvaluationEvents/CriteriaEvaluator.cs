using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.EntityFramework;
using EvaluationCriteria = PlayGen.SUGAR.Server.Model.EvaluationCriteria;

namespace PlayGen.SUGAR.Server.Core.EvaluationEvents
{
	/// <summary>
	/// Evaluates evaluation criteria.
	/// </summary>
	public class CriteriaEvaluator
	{
		protected readonly GroupMemberController GroupMemberCoreController;
		protected readonly UserFriendController UserFriendCoreController;
	    protected readonly SUGARContextFactory ContextFactory;

		// todo change all db controller usage to core controller usage
		public CriteriaEvaluator(SUGARContextFactory contextFactory, GroupMemberController groupMemberCoreController, UserFriendController userFriendCoreController)
		{
		    ContextFactory = contextFactory;
			GroupMemberCoreController = groupMemberCoreController;
			UserFriendCoreController = userFriendCoreController;
		}

		// TODO: currently this is binary but should eventually return a progress value
		// The method of returning calculating the progress (for multiple criteria conditions) and 
		// how the progress is going to be represented (0f to 1f ?) need to be determined first.
		public float IsCriteriaSatisified(int? gameId, int? actorId, List<EvaluationCriteria> completionCriterias, ActorType actorType, EvaluationType evaluationType)
		{
			return completionCriterias.Sum(cc => Evaluate(gameId, actorId, cc, actorType, evaluationType)) / completionCriterias.Count;
		}

		protected float Evaluate(int? gameId, int? actorId, EvaluationCriteria completionCriteria, ActorType actorType, EvaluationType evaluationType)
		{
			var category = evaluationType == EvaluationType.Achievement
				? EvaluationDataCategory.Achievement
				: EvaluationDataCategory.Skill;

			if (completionCriteria.Scope == CriteriaScope.RelatedActors && actorId != null)
			{
				if (actorType != ActorType.Group)
				{
					throw new NotImplementedException("RelatedActors Scope is only implemented for groups");
				}
				var groupActors = GroupMemberCoreController.GetMembers(actorId.Value).ToList<Actor>();
				switch (completionCriteria.EvaluationDataType)
				{
					case EvaluationDataType.Boolean:
						return EvaluateManyBool(gameId, groupActors, completionCriteria, category);

					case EvaluationDataType.String:
						return EvaluateManyString(gameId, groupActors, completionCriteria, category);

					case EvaluationDataType.Float:
						return EvaluateManyFloat(gameId, groupActors, completionCriteria, category);

					case EvaluationDataType.Long:
						return EvaluateManyLong(gameId, groupActors, completionCriteria, category);

					default:
						return 0;
				}
			}
			else
			{
				switch (completionCriteria.EvaluationDataType)
				{
					case EvaluationDataType.Boolean:
						return EvaluateBool(gameId, actorId, completionCriteria, category);

					case EvaluationDataType.String:
						return EvaluateString(gameId, actorId, completionCriteria, category);

					case EvaluationDataType.Float:
						return EvaluateFloat(gameId, actorId, completionCriteria, category);

					case EvaluationDataType.Long:
						return EvaluateLong(gameId, actorId, completionCriteria, category);

					default:
						return 0;
				}
			}
		}

		protected float EvaluateLong(int? gameId, int? actorId, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            var evaluationDataController = new EvaluationDataController(ContextFactory, completionCriteria.EvaluationDataCategory);

			switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.AllLongs(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Long, evaluationDataCategory).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(value, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Sum:
					var sum = evaluationDataController.SumLongs(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Long, evaluationDataCategory);

					return CompareValues(sum, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					long latest;
					if (!evaluationDataController.TryGetLatestLong(gameId, actorId, completionCriteria.EvaluationDataKey, out latest, EvaluationDataType.Long, evaluationDataCategory))
					{
                        return 0;
					}

					return CompareValues(latest, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateFloat(int? gameId, int? actorId, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            var evaluationDataController = new EvaluationDataController(ContextFactory, completionCriteria.EvaluationDataCategory);

            switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.AllFloats(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Float, evaluationDataCategory).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(value, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Sum:
					var sum = evaluationDataController.SumFloats(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Float, evaluationDataCategory);

					return CompareValues(sum, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					float latest;
					if (!evaluationDataController.TryGetLatestFloat(gameId, actorId, completionCriteria.EvaluationDataKey, out latest, EvaluationDataType.Float, evaluationDataCategory))
					{
                        return 0;
                    }

					return CompareValues(latest, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateString(int? gameId, int? actorId, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            var evaluationDataController = new EvaluationDataController(ContextFactory, completionCriteria.EvaluationDataCategory);

            switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.AllStrings(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.String, evaluationDataCategory).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(value, completionCriteria.Value, completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Latest:
					string latest;
					if (!evaluationDataController.TryGetLatestString(gameId, actorId, completionCriteria.EvaluationDataKey, out latest, EvaluationDataType.String, evaluationDataCategory))
					{
                        return 0;
                    }

					return CompareValues(latest, completionCriteria.Value, completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateBool(int? gameId, int? actorId, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            var evaluationDataController = new EvaluationDataController(ContextFactory, completionCriteria.EvaluationDataCategory);

            switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					var any = evaluationDataController.AllBools(gameId, actorId, completionCriteria.EvaluationDataKey, EvaluationDataType.Boolean, evaluationDataCategory ).ToList();

					return !any.Any() ? 0 : any.Max(value => CompareValues(value, bool.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType));
				case CriteriaQueryType.Latest:
					bool latest;
					if (!evaluationDataController.TryGetLatestBool(gameId, actorId, completionCriteria.EvaluationDataKey, out latest, EvaluationDataType.Boolean, evaluationDataCategory))
					{
                        return 0;
                    }

					return CompareValues(latest, bool.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				default:
					return 0;
			}
		}

		protected float EvaluateManyLong(int? gameId, List<Actor> actors, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            var evaluationDataController = new EvaluationDataController(ContextFactory, completionCriteria.EvaluationDataCategory);

		    switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					return actors.Sum(a => EvaluateLong(gameId, a.Id, completionCriteria, evaluationDataCategory)) / actors.Count;
				case CriteriaQueryType.Sum:
					var sum = actors.Sum(a => evaluationDataController.SumLongs(gameId, a.Id, completionCriteria.EvaluationDataKey, EvaluationDataType.Long, evaluationDataCategory));

					return CompareValues(sum, long.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					return actors.Sum(a => EvaluateLong(gameId, a.Id, completionCriteria, evaluationDataCategory)) / actors.Count;
				default:
					return 0;
			}
		}

		protected float EvaluateManyFloat(int? gameId, List<Actor> actors, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            var evaluationDataController = new EvaluationDataController(ContextFactory, completionCriteria.EvaluationDataCategory);
            
            switch (completionCriteria.CriteriaQueryType)
			{
				case CriteriaQueryType.Any:
					return actors.Sum(a => EvaluateFloat(gameId, a.Id, completionCriteria, evaluationDataCategory)) / actors.Count;
				case CriteriaQueryType.Sum:
					var sum = actors.Sum(a => evaluationDataController.SumFloats(gameId, a.Id, completionCriteria.EvaluationDataKey, EvaluationDataType.Float, evaluationDataCategory));

					return CompareValues(sum, float.Parse(completionCriteria.Value), completionCriteria.ComparisonType, completionCriteria.EvaluationDataType);
				case CriteriaQueryType.Latest:
					return actors.Sum(a => EvaluateFloat(gameId, a.Id, completionCriteria, evaluationDataCategory)) / actors.Count;
				default:
					return 0;
			}
		}

		protected float EvaluateManyString(int? gameId, List<Actor> actors, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            return actors.Sum(a => EvaluateString(gameId, a.Id, completionCriteria, evaluationDataCategory)) / actors.Count;
		}

		protected float EvaluateManyBool(int? gameId, List<Actor> actors, EvaluationCriteria completionCriteria, EvaluationDataCategory evaluationDataCategory)
		{
            return actors.Sum(a => EvaluateBool(gameId, a.Id, completionCriteria, evaluationDataCategory)) / actors.Count;
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
