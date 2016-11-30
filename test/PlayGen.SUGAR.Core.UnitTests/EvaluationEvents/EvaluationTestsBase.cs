using System;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Core.Sessions;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    public abstract class EvaluationTestsBase : IDisposable
    {
        protected readonly EvaluationTracker EvaluationTracker;
        protected readonly ProgressEvaluator ProgressEvaluator;
        protected readonly SessionTracker SessionTracker = new SessionTracker(new TimeSpan(0, 2, 0));

        protected EvaluationTestsBase()
        {
            ProgressEvaluator = new ProgressEvaluator(ControllerLocator.EvaluationController);
            EvaluationTracker = new EvaluationTracker(ProgressEvaluator, ControllerLocator.EvaluationController, SessionTracker);
        }

        public void Dispose()
        {
        }
    }
}