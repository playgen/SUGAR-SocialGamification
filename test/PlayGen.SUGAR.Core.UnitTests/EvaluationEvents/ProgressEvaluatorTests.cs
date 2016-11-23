
using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Core.EvaluationEvents;
using Xunit;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
	[Collection("Test Data Fixture Collection")] // todo change to use this one
	//[Collection("Project Fixture Collection")] // having 2 different kinds of project fixtures seems to cause issues and tests fail where they would normally pass.
	public class ProgressEvaluatorTests
	{
		private readonly ProgressEvaluator _progressEvaluator;

		public ProgressEvaluatorTests()
		{
			var criteriaEvaluator = new CriteriaEvaluator(ControllerLocator.GameDataController, 
				ControllerLocator.GroupMemberController, 
				ControllerLocator.UserFriendController);

			_progressEvaluator = new ProgressEvaluator(criteriaEvaluator);
		}

		[Fact]
		public void CanEvaluateActor()
		{
			// Assign
		    var evaluationCount = 2;
			var actor = Helpers.GetOrCreateUser("CanEvaluateActor");

            var evaluations = new List<Evaluation>();

		    for (var i = 0; i < evaluationCount; i++)
		    {
                var evaluation = Helpers.CreateGenericAchievement($"CanEvaluateActorComplete_{i}");
                Helpers.CompleteGenericAchievement(evaluation, actor.Id);
                evaluations.Add(evaluation);
            }
			
			// Act
			var progress = _progressEvaluator.EvaluateActor(evaluations, evaluations[0].GameId, actor);

            // Assert
            Assert.Equal(evaluations.Count, progress.Count); // Should have a progress value for each evaluation

		    foreach (var evaluation in evaluations)
		    {
                Assert.Contains(evaluation, progress.Keys); // Each evaluation should be returned in the progress
                Assert.True(progress[evaluation] > 0);      // Make sure actually processed progress
            }
        }

        [Fact]
		public void CanRemoveActor()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanGetProgress()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanEvaluateEvaluation()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanEvaluateEvaluations()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanRemoveEvaluationProgress()
		{
			throw new NotImplementedException();
		}
	}
}