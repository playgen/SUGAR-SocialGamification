using System;
using Microsoft.Extensions.Logging.Abstractions;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.Core.Sessions;

namespace PlayGen.SUGAR.Server.Core.Tests.EvaluationEvents
{
    public abstract class EvaluationTestsBase : CoreTestBase
    {
        protected readonly EvaluationTracker EvaluationTracker;
        protected readonly ProgressEvaluator ProgressEvaluator;
        protected readonly SessionTracker SessionTracker = new SessionTracker(new NullLogger<SessionTracker>(), new TimeSpan(0, 2, 0), new TimeSpan(0, 10, 0));

        protected EvaluationTestsBase()
        {
            ProgressEvaluator = new ProgressEvaluator(ControllerLocator.EvaluationController);
            EvaluationTracker = new EvaluationTracker(ProgressEvaluator, ControllerLocator.EvaluationController, SessionTracker);
        }
    }
}