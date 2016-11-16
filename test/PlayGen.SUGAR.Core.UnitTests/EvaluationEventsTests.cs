using System;
using PlayGen.SUGAR.Core.EvaluationEvents;
using Xunit;

namespace PlayGen.SUGAR.Core.UnitTests
{
    [Collection("Project Fixture Collection")]
    public class EvaluationEventsTests : IDisposable
    {
        private readonly EvaluationTracker _evaluationTracker;
        private readonly EvaluationGameDataMapper _gameDataMapper;
        private readonly ProgressCache _progressCache;
        private readonly ProgressNotificationCache _progressNotificationCache;
        private readonly CriteriaEvaluator _criteriaEvaluator;

        public EvaluationEventsTests()
        {
            _criteriaEvaluator = new CriteriaEvaluator(ControllerLocator.GameDataController, 
                ControllerLocator.GroupMemberController, ControllerLocator.UserFriendController);

            _gameDataMapper = new EvaluationGameDataMapper();
            _progressCache = new ProgressCache(_criteriaEvaluator);
            _progressNotificationCache = new ProgressNotificationCache();

            _evaluationTracker = new EvaluationTracker(_gameDataMapper, _progressCache, _progressNotificationCache);
        }

        public void Dispose()
        {

        }
    }
}
