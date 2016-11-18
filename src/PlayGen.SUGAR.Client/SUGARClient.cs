using PlayGen.SUGAR.Client.EvaluationEvents;

namespace PlayGen.SUGAR.Client
{
	public class SUGARClient
	{
		private readonly IHttpHandler _httpHandler;

		private readonly string _baseAddress;
		private readonly Credentials _credentials = new Credentials();
        private readonly EvaluationNotifications _evaluationNotifications = new EvaluationNotifications();

		private AccountClient _accountClient;
		private AchievementClient _achievementClient;
		private GameClient _gameClient;
		private GameDataClient _gameDataClient;
		private GroupClient _groupClient;
		private GroupMemberClient _groupMemberClient;
		private UserClient _userClient;
		private UserFriendClient _userFriendClient;
		private ResourceClient _resourceClient;
		private LeaderboardClient _leaderboardClient;
		private SkillClient _skillClient;

		public AccountClient Account			=> _accountClient ?? (_accountClient = new AccountClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public AchievementClient Achievement	=> _achievementClient ?? (_achievementClient = new AchievementClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public GameClient Game					=> _gameClient ?? (_gameClient = new GameClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public GameDataClient GameData			=> _gameDataClient ?? (_gameDataClient = new GameDataClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public GroupClient Group				=> _groupClient ?? (_groupClient = new GroupClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public GroupMemberClient GroupMember	=> _groupMemberClient ?? (_groupMemberClient = new GroupMemberClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public UserClient User					=> _userClient ?? (_userClient = new UserClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public UserFriendClient UserFriend		=> _userFriendClient ?? (_userFriendClient = new UserFriendClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public ResourceClient Resource			=> _resourceClient ?? (_resourceClient = new ResourceClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public LeaderboardClient Leaderboard	=> _leaderboardClient ?? (_leaderboardClient = new LeaderboardClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
		public SkillClient Skill				=> _skillClient ?? (_skillClient = new SkillClient(_baseAddress, _credentials, _httpHandler, _evaluationNotifications));
	    
        public SUGARClient(string baseAddress, IHttpHandler httpHandler = null)
		{
			_baseAddress = baseAddress;
			_httpHandler = httpHandler ?? new DefaultHttpHandler();
		}
	}
}
