using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;

namespace PlayGen.SUGAR.Client
{
	public class SUGARClient
	{
		protected readonly string _baseAddress;
		protected readonly IHttpHandler _httpHandler;
		protected readonly Dictionary<string, string> _persistentHeaders;
		protected readonly Dictionary<string, string> _sessionHeaders;
		protected readonly AsyncRequestController _asyncRequestController;
		protected readonly EvaluationNotifications _evaluationNotifications = new EvaluationNotifications();

		private APIVersionClient _apiVersionClient;
		private AccountClient _accountClient;
		private SessionClient _sessionClient;
		private AchievementClient _achievementClient;
		private GameClient _gameClient;
		private GameDataClient _gameDataClient;
		private GroupClient _groupClient;
		private GroupMemberClient _groupMemberClient;
		private UserClient _userClient;
		private UserFriendClient _userFriendClient;
		private AllianceClient _allianceClient;
		private ResourceClient _resourceClient;
		private LeaderboardClient _leaderboardClient;
		private SkillClient _skillClient;
		private MatchClient _matchClient;
		
		public APIVersionClient APIVersion => _apiVersionClient ?? (_apiVersionClient = new APIVersionClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public AccountClient Account => _accountClient ?? (_accountClient = new AccountClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public SessionClient Session => _sessionClient ?? (_sessionClient = new SessionClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public AchievementClient Achievement => _achievementClient ?? (_achievementClient = new AchievementClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public GameClient Game => _gameClient ?? (_gameClient = new GameClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public GameDataClient GameData => _gameDataClient ?? (_gameDataClient = new GameDataClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public GroupClient Group => _groupClient ?? (_groupClient = new GroupClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public GroupMemberClient GroupMember => _groupMemberClient ?? (_groupMemberClient = new GroupMemberClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public UserClient User => _userClient ?? (_userClient = new UserClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public UserFriendClient UserFriend => _userFriendClient ?? (_userFriendClient = new UserFriendClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public AllianceClient AllianceClient => _allianceClient ?? (_allianceClient = new AllianceClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public ResourceClient Resource => _resourceClient ?? (_resourceClient = new ResourceClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public LeaderboardClient Leaderboard => _leaderboardClient ?? (_leaderboardClient = new LeaderboardClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public SkillClient Skill => _skillClient ?? (_skillClient = new SkillClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));
		public MatchClient Match => _matchClient ?? (_matchClient = new MatchClient(_baseAddress, _httpHandler, _persistentHeaders, _sessionHeaders, _asyncRequestController, _evaluationNotifications));

		// todo possibly pass update event so async requests are either read in and handled there or this creates another thread to poll the async queue
		public SUGARClient(string baseAddress, IHttpHandler httpHandler = null, Dictionary<string, string> persistentHeaders = null, Dictionary<string, string> sessionHeaders = null, int timeoutMilliseconds = 60 * 1000)
		{
			_baseAddress = baseAddress;
			_httpHandler = httpHandler ?? new DefaultHttpHandler();
			_persistentHeaders = persistentHeaders ?? new Dictionary<string, string> { { Common.APIVersion.Key, Common.APIVersion.Version } };
			_sessionHeaders = sessionHeaders ?? new Dictionary<string, string>();
			_asyncRequestController = new AsyncRequestController(timeoutMilliseconds, () => Session.Heartbeat());
		}

		public bool TryExecuteResponse()
		{
			return _asyncRequestController.TryExecuteResponse();
		}
	}
}
