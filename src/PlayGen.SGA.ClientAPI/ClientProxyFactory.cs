using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SGA.ClientAPI
{
    public class ClientProxyFactory
    {
        private readonly string _baseAddress;

        public AccountClientProxy GetAccountClientProxy => new AccountClientProxy(_baseAddress);
        public GameClientProxy GetGameClientProxy => new GameClientProxy(_baseAddress);
        public GroupAchievementClientProxy GetGroupAchievementClientProxy => new GroupAchievementClientProxy(_baseAddress);
        public GroupClientProxy GetGroupClientProxy => new GroupClientProxy(_baseAddress);
        public GroupMemberClientProxy GetGroupMemberClientProxy => new GroupMemberClientProxy(_baseAddress);
        public GroupSaveDataClientProxy GetGroupSaveDataClientProxy => new GroupSaveDataClientProxy(_baseAddress);
        public UserAchievementClientProxy GetUserAchievementClientProxy => new UserAchievementClientProxy(_baseAddress);
        public UserClientProxy GetUserClientProxy => new UserClientProxy(_baseAddress);
        public UserFriendClientProxy GetUserFriendClientProxy => new UserFriendClientProxy(_baseAddress);
        public UserSaveDataClientProxy GetUserSaveDataClientProxy => new UserSaveDataClientProxy(_baseAddress);

        public ClientProxyFactory(string baseAddress)
        {
            _baseAddress = baseAddress;
        }
    }
}
