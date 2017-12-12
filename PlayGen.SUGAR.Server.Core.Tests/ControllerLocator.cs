using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PlayGen.SUGAR.Server.Core.Authorization;
using PlayGen.SUGAR.Server.Core.Controllers;
using DbControllerLocator = PlayGen.SUGAR.Server.EntityFramework.Tests.ControllerLocator;

namespace PlayGen.SUGAR.Server.Core.Tests
{
    public abstract class ControllerLocator
    {
        private static AccountController _accountController;
        private static AccountSourceController _accountSourceController;
		private static ActorClaimController _actorClaimController;
		private static ActorRoleController _actorRoleController;
        private static ClaimController _claimController;
        private static EvaluationController _evaluationController;
        private static GameController _gameController;
        private static EvaluationDataController _evaluationDataController;
        private static GroupController _groupController;
        private static LeaderboardController _leaderboardController;
        private static ResourceController _resourceController;
        private static RewardController _rewardController;
        private static RoleController _roleController;
        private static RoleClaimController _roleClaimController;
        private static UserController _userController;
        private static MatchController _matchController;
        private static ActorController _actorController;
        private static GameDataController _gameDataController;
	    private static RelationshipController _relationshipController;

        public static AccountController AccountController
            => _accountController ?? (_accountController = new AccountController(new NullLogger<AccountController>(), DbControllerLocator.AccountController, AccountSourceController, UserController, ActorRoleController));

        public static AccountSourceController AccountSourceController
            => _accountSourceController ?? (_accountSourceController = new AccountSourceController(new NullLogger<AccountSourceController>(), DbControllerLocator.AccountSourceController));

		public static ActorClaimController ActorClaimController
			=> _actorClaimController ?? (_actorClaimController = new ActorClaimController(new NullLogger<ActorClaimController>(), DbControllerLocator.ActorClaimController, ActorRoleController, RoleClaimController));

		public static ActorRoleController ActorRoleController
            => _actorRoleController ?? (_actorRoleController = new ActorRoleController(new NullLogger<ActorRoleController>(), DbControllerLocator.ActorRoleController, DbControllerLocator.RoleController));

        public static ClaimController ClaimController
            => _claimController ?? (_claimController = new ClaimController(DbControllerLocator.ClaimController, DbControllerLocator.RoleController, DbControllerLocator.RoleClaimController));

        public static EvaluationController EvaluationController
            => _evaluationController ?? (_evaluationController= new EvaluationController(new NullLogger<EvaluationController>(), new NullLogger<EvaluationDataController>(), DbControllerLocator.EvaluationController, RelationshipController, ActorController, RewardController, DbControllerLocator.ContextFactory));

        public static GameController GameController
            => _gameController ?? (_gameController = new GameController(new NullLogger<GameController>(), DbControllerLocator.GameController, ActorClaimController, ActorRoleController));
        
        public static GroupController GroupController
            => _groupController ?? (_groupController = new GroupController(new NullLogger<GroupController>(), DbControllerLocator.GroupController, DbControllerLocator.ActorController, ActorClaimController, ActorRoleController, RelationshipController));

        public static LeaderboardController LeaderboardController
            => _leaderboardController ?? (_leaderboardController = new LeaderboardController(new NullLogger<LeaderboardController>(), new NullLogger<EvaluationDataController>(), DbControllerLocator.LeaderboardController, RelationshipController, ActorController, GroupController, UserController, DbControllerLocator.ContextFactory));

        public static ResourceController ResourceController
            => _resourceController ?? (_resourceController = new ResourceController(new NullLogger<ResourceController>(), new NullLogger<EvaluationDataController>(), DbControllerLocator.ContextFactory));

        public static RewardController RewardController
            => _rewardController ?? (_rewardController = new RewardController(new NullLogger<RewardController>(), new NullLogger<EvaluationDataController>(), DbControllerLocator.ContextFactory, RelationshipController));

        public static RoleController RoleController
            => _roleController ?? (_roleController = new RoleController(new NullLogger<RoleController>(), DbControllerLocator.RoleController, ActorRoleController));

        public static RoleClaimController RoleClaimController
            => _roleClaimController ?? (_roleClaimController = new RoleClaimController(new NullLogger<RoleClaimController>(), DbControllerLocator.RoleClaimController));

        public static UserController UserController
            => _userController ?? (_userController = new UserController(new NullLogger<UserController>(), DbControllerLocator.UserController, DbControllerLocator.ActorController, ActorRoleController));

        public static MatchController MatchController
            => _matchController ?? (_matchController = new MatchController(new NullLogger<MatchController>(), new NullLogger<EvaluationDataController>(), DbControllerLocator.ContextFactory, DbControllerLocator.MatchController));

        public static ActorController ActorController
            => _actorController ?? (_actorController = new ActorController(DbControllerLocator.ActorController));

        public static GameDataController GameDataController =>
            _gameDataController ?? (_gameDataController = new GameDataController(new NullLogger<GameDataController>(), new NullLogger<EvaluationDataController>(), DbControllerLocator.ContextFactory, ActorController));

	    public static RelationshipController RelationshipController
		    => _relationshipController ?? (_relationshipController =
					new RelationshipController(new NullLogger<RelationshipController>(), DbControllerLocator.RelationshipController));

    }
}
