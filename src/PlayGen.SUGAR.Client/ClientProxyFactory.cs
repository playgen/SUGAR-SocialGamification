namespace PlayGen.SUGAR.Client
{
	public class ClientProxyFactory
	{
		private readonly string _baseAddress;

		public AccountClient GetAccountClient => new AccountClient(_baseAddress);
		public GameClient GetGameClient => new GameClient(_baseAddress);
		public GroupAchievementClient GetGroupAchievementClient => new GroupAchievementClient(_baseAddress);
		public GroupClient GetGroupClient => new GroupClient(_baseAddress);
		public GroupMemberClient GetGroupMemberClient => new GroupMemberClient(_baseAddress);
		public GroupDataClient GetGroupSaveDataClient => new GroupDataClient(_baseAddress);
		public UserAchievementClient GetUserAchievementClient => new UserAchievementClient(_baseAddress);
		public UserClient GetUserClient => new UserClient(_baseAddress);
		public UserFriendClient GetUserFriendClient => new UserFriendClient(_baseAddress);
		public UserDataClient GetUserSaveDataClient => new UserDataClient(_baseAddress);

		public ClientProxyFactory(string baseAddress)
		{
			_baseAddress = baseAddress;
		}
	}
}
