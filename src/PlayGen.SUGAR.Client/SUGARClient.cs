namespace PlayGen.SUGAR.Client
{
	public class SUGARClient
	{
		private readonly string _baseAddress;
		private readonly Credentials _credentials = new Credentials();

		private AccountClient _accountClient;
		private GameClient _gameClient;
		private GroupAchievementClient _groupAchievementClient;
		private GroupClient _groupClient;
		private GroupMemberClient _groupMemberClient;
		private GroupDataClient _groupDataClient;
		private UserAchievementClient _userAchievementClient;
		private UserClient _userClient;
		private UserFriendClient _userFriendClient;
		private UserDataClient _userDataClient;

		public AccountClient Account					=> _accountClient ?? (_accountClient = new AccountClient(_baseAddress, _credentials));
		public GameClient Game							=> _gameClient ?? (_gameClient = new GameClient(_baseAddress, _credentials));
		public GroupAchievementClient GroupAchievement	=> _groupAchievementClient ?? (_groupAchievementClient = new GroupAchievementClient(_baseAddress, _credentials));
		public GroupClient Group						=> _groupClient ?? (_groupClient = new GroupClient(_baseAddress, _credentials));
		public GroupMemberClient GroupMember			=> _groupMemberClient ?? (_groupMemberClient = new GroupMemberClient(_baseAddress, _credentials));
		public GroupDataClient GroupData				=> _groupDataClient ?? (_groupDataClient = new GroupDataClient(_baseAddress, _credentials));
		public UserAchievementClient UserAchievement	=> _userAchievementClient ?? (_userAchievementClient = new UserAchievementClient(_baseAddress, _credentials));
		public UserClient User							=> _userClient ?? (_userClient = new UserClient(_baseAddress, _credentials));
		public UserFriendClient UserFriend				=> _userFriendClient ?? (_userFriendClient = new UserFriendClient(_baseAddress, _credentials));
		public UserDataClient UserData					=> _userDataClient ?? (_userDataClient = new UserDataClient(_baseAddress, _credentials));


		public SUGARClient(string baseAddress)
		{
			_baseAddress = baseAddress;
		}
	}
}
