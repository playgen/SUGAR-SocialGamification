using System;
using PlayGen.SUGAR.Core.EvaluationEvents;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests.EvaluationEvents
{
    public abstract class TestsBase : IDisposable
    {
        protected readonly EvaluationTracker EvaluationTracker;
        protected readonly EvaluationGameDataMapper GameDataMapper;
        protected readonly ProgressEvaluator ProgressEvaluator;
        protected readonly ProgressNotificationCache ProgressNotificationCache;
        protected readonly CriteriaEvaluator CriteriaEvaluator;

        protected TestsBase()
        {
            CriteriaEvaluator = new CriteriaEvaluator(ControllerLocator.GameDataController,
                ControllerLocator.GroupMemberController, ControllerLocator.UserFriendController);

            GameDataMapper = new EvaluationGameDataMapper();
            ProgressEvaluator = new ProgressEvaluator(CriteriaEvaluator);
            ProgressNotificationCache = new ProgressNotificationCache();

            EvaluationTracker = new EvaluationTracker(GameDataMapper, ProgressEvaluator, ProgressNotificationCache);
        }

        public void Dispose()
        {
        }
    }
}