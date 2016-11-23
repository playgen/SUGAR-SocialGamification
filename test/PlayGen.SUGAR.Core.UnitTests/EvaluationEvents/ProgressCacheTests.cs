
using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Core.EvaluationEvents;
using Xunit;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    //[Collection("Project Fixture Collection")]
    public class ProgressCacheTests
	{
		private readonly ProgressEvaluator _progressEvaluator;

		public ProgressCacheTests()
		{
			var criteriaEvaluator = new CriteriaEvaluator(ControllerLocator.GameDataController, 
				ControllerLocator.GroupMemberController, 
				ControllerLocator.UserFriendController);

			_progressEvaluator = new ProgressEvaluator(criteriaEvaluator);
		}

		[Fact]
		public void CanStartTracking()
		{
			// Assign
			var actor = Helpers.GetOrCreateUser("CanStartTracking");

			var evaluation = Helpers.CreateGenericAchievement("CanStartTracking");
			Helpers.CompleteGenericAchievement(evaluation, actor.Id);

			// Act
			_progressEvaluator.StartTracking(evaluation.GameId, actor.Id);

			// Assert
			Dictionary<Evaluation, float> actorEvaluationProgress;
		    var didGetprogress = false;
		    var didGetSpecificEvaluation = false;

		    while (_progressEvaluator.TryGetProgress(evaluation.GameId, actor.Id, out actorEvaluationProgress))
		    {
		        didGetprogress = true;
                Assert.NotNull(actorEvaluationProgress);

		        didGetSpecificEvaluation |= actorEvaluationProgress.Keys.Any(e => e.Token == evaluation.Token);
		    }

            Assert.True(didGetprogress);
            Assert.True(didGetSpecificEvaluation);
		}

		[Fact]
		public void CanStopTracking()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanGetProgress()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CanEvaluateActor()
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