using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
    /// <summary>
	/// Assumptions:
	/// Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
	/// Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
    /// </summary>
    public class EvaluationControllerTests : CoreTestFixtureBase
    {
        private readonly Controllers.EvaluationController _evaluationController = ControllerLocator.EvaluationController;

        public EvaluationControllerTests(CoreTestFixture fixture) : base(fixture)
        {
        }

        [Theory]
        [InlineData(EvaluationDataType.String, 10)]
        [InlineData(EvaluationDataType.Long, 10)]
        [InlineData(EvaluationDataType.Float, 10)]
        [InlineData(EvaluationDataType.Boolean, 10)]
        public void CanGetCountCompletedProgressForUser(EvaluationDataType evaluationDataType, int userIndex)
        {
            // Arrange
            var token = $"{nameof(CanGetCountCompletedProgressForUser)}_{evaluationDataType}";

            var user = Fixture.SortedUsers[userIndex];
            var userDataCount = userIndex + 1;

			var evaluation = CreateCountEvaluation(
				token,
				user.ActorType,
				evaluationDataType,
				CriteriaScope.Actor,
				userDataCount);

            // Act
            var result = _evaluationController.EvaluateProgress(evaluation, user.Id);

            // Assert
            Assert.Equal(1, result);
        }

		[Theory]
        [InlineData(EvaluationDataType.String, 10)]
        [InlineData(EvaluationDataType.Long, 10)]
        [InlineData(EvaluationDataType.Float, 10)]
        [InlineData(EvaluationDataType.Boolean, 10)]
        public void CanGetCountIncompleteProgressForUser(EvaluationDataType evaluationDataType, int userIndex)
        {
            // Arrange
            var token = $"{nameof(CanGetCountIncompleteProgressForUser)}_{evaluationDataType}";

            var user = Fixture.SortedUsers[userIndex];
            var userDataCount = userIndex + 1;
            var doubleUserDataCount = userDataCount * 2;

			var evaluation = CreateCountEvaluation(
				token,
				user.ActorType,
				evaluationDataType,
				CriteriaScope.Actor,
				doubleUserDataCount);

            // Act
            var result = _evaluationController.EvaluateProgress(evaluation, user.Id);

            // Assert
            Assert.Equal((float)userDataCount / doubleUserDataCount, result);
        }

        [Theory]
        [InlineData(EvaluationDataType.String, 1)]
        [InlineData(EvaluationDataType.Long, 1)]
        [InlineData(EvaluationDataType.Float, 1)]
        [InlineData(EvaluationDataType.Boolean, 1)]
        public void CanGetCountCompletedProgressForGroupMembers(EvaluationDataType evaluationDataType, int groupIndex)
        {
            // Arrange
            var token = $"{nameof(CanGetCountCompletedProgressForGroupMembers)}_{evaluationDataType}";

            var group = Fixture.SortedGroups[groupIndex];
            var groupUsers = ControllerLocator.RelationshipController.GetRelatedActors(group.Id, ActorType.User);

            // Each group user that was seeded using the CoreTestFixture should have
            // its index in sorted users + 1 as the count of data for a given type.
            var groupUsersDataCount = groupUsers
                .Sum(gu => Fixture.SortedUsers
                                .ToList()
                                .FindIndex(su => su.Id == gu.Id) + 1);

			var evaluation = CreateCountEvaluation(
				token,
				group.ActorType,
				evaluationDataType,
				CriteriaScope.RelatedUsers,
				groupUsersDataCount);

            // Act
            var result = _evaluationController.EvaluateProgress(evaluation, group.Id);

            // Assert
            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(EvaluationDataType.String, 1)]
        [InlineData(EvaluationDataType.Long, 1)]
        [InlineData(EvaluationDataType.Float, 1)]
        [InlineData(EvaluationDataType.Boolean, 1)]
        public void CanGetCountIncompleteProgressForGroupMembers(EvaluationDataType evaluationDataType, int groupIndex)
        {
            // Arrange
            var token = $"{nameof(CanGetCountIncompleteProgressForGroupMembers)}_{evaluationDataType}";

            var group = Fixture.SortedGroups[groupIndex];
            var groupUsers = ControllerLocator.RelationshipController.GetRelatedActors(group.Id, ActorType.User);

            // Each group user that was seeded using the CoreTestFixture should have
            // its index in sorted users + 1 as the count of data for a given type.
            var groupUsersDataCount = groupUsers
                .Sum(gu => Fixture.SortedUsers
                                .ToList()
                                .FindIndex(su => su.Id == gu.Id) + 1);
            var doubleGroupUserDataCount = groupUsersDataCount * 2;

			var evaluation = CreateCountEvaluation(
				token, 
				group.ActorType, 
				evaluationDataType,
				CriteriaScope.RelatedUsers, 
				doubleGroupUserDataCount);
			
            // Act
            var result = _evaluationController.EvaluateProgress(evaluation, group.Id);

            // Assert
            Assert.Equal((float)groupUsersDataCount / doubleGroupUserDataCount, result);
        }

		// todo scopes for user and group

        #region Helpers
        private Evaluation CreateCountEvaluation(string token, ActorType actorType, EvaluationDataType evaluationDataType, CriteriaScope scope, int value)
		{
			return _evaluationController.Create(
				new Achievement
				{
					GameId = Fixture.EvaluationDataGameId,
					Name = $"{token} Name",
					Description = $"{token} Description",
					ActorType = actorType,
					Token = token,
					EvaluationCriterias = new List<EvaluationCriteria>
					{
						new EvaluationCriteria
						{
							EvaluationDataKey = Fixture.GenerateEvaluationDataKey(evaluationDataType),
							EvaluationDataCategory = EvaluationDataCategory.GameData,
							EvaluationDataType = evaluationDataType,
							CriteriaQueryType = CriteriaQueryType.Count,
							ComparisonType = ComparisonType.GreaterOrEqual,
							Scope = scope,
							Value = value.ToString()
						}
					}
				});
		}
        #endregion
    }
}