using System;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Core.Sessions;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    public abstract class EvaluationTestsBase : IDisposable
    {
        protected readonly EvaluationTracker EvaluationTracker;
        protected readonly ProgressEvaluator ProgressEvaluator;
        protected readonly CriteriaEvaluator CriteriaEvaluator;
        protected readonly SessionTracker SessionTracker = new SessionTracker();

        protected EvaluationTestsBase()
        {
            CriteriaEvaluator = new CriteriaEvaluator(ControllerLocator.GameDataController,
                ControllerLocator.GroupMemberController, ControllerLocator.UserFriendController);
            ProgressEvaluator = new ProgressEvaluator(CriteriaEvaluator);

            EvaluationTracker = new EvaluationTracker(ProgressEvaluator, ControllerLocator.EvaluationController, SessionTracker);
        }

        public void Dispose()
        {
        }
    }
}