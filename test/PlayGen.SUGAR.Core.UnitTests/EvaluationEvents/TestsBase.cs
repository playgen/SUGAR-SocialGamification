using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Core.EvaluationEvents;
using PlayGen.SUGAR.Core.UnitTests;
using PlayGen.SUGAR.Data.Model;
using Xunit;
using EvaluationCriteria = PlayGen.SUGAR.Data.Model.EvaluationCriteria;

namespace PlayGen.SUGAR.Core.UnitTests
{
    [Collection("Project Fixture Collection")]
    public abstract class TestsBase : IDisposable
    {
        protected readonly EvaluationTracker EvaluationTracker;
        protected readonly EvaluationGameDataMapper GameDataMapper;
        protected readonly ProgressCache ProgressCache;
        protected readonly ProgressNotificationCache ProgressNotificationCache;
        protected readonly CriteriaEvaluator CriteriaEvaluator;

        protected TestsBase()
        {
            CriteriaEvaluator = new CriteriaEvaluator(ControllerLocator.GameDataController,
                ControllerLocator.GroupMemberController, ControllerLocator.UserFriendController);

            GameDataMapper = new EvaluationGameDataMapper();
            ProgressCache = new ProgressCache(CriteriaEvaluator);
            ProgressNotificationCache = new ProgressNotificationCache();

            EvaluationTracker = new EvaluationTracker(GameDataMapper, ProgressCache, ProgressNotificationCache);
        }

        public void Dispose()
        {
        }

        #region Helpers

        protected Evaluation CreateEvaluation(string name, int gameId)
        {
            var evaluationCriterias = new List<EvaluationCriteria>();
            for (var i = 0; i < 2; i++)
            {
                evaluationCriterias.Add(new EvaluationCriteria
                {
                    Id = 1,
                    Key = $"{name}_{i}",
                    DataType = GameDataType.Long,
                    CriteriaQueryType = CriteriaQueryType.Sum,
                    ComparisonType = ComparisonType.GreaterOrEqual,
                    Scope = CriteriaScope.Actor,
                    Value = "100"
                });
            }

            return new Data.Model.Achievement
            {
                // Arrange
                Id = 1,
                Token = name,

                Name = name,
                Description = name,

                ActorType = ActorType.User,
                GameId = gameId,

                EvaluationCriterias = evaluationCriterias
            };
        }

        protected GameData CreateGameData(EvaluationCriteria evaluationCriteria, int gameId)
        {
            return new GameData
            {
                Key = evaluationCriteria.Key,
                DataType = evaluationCriteria.DataType,

                GameId = gameId,
                Value = "50"
            };
        }
    }
    #endregion
}