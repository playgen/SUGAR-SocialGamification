using System;
using System.Diagnostics;
using System.Linq;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.Tests
{
    public static class Helpers
    {
        public static ActorResponse GetOrCreateUser(UserClient userClient, string name)
        {
            var users = userClient.Get(name, true);
            ActorResponse user;

            if (users.Any())
            {
                user = users.Single();
            }
            else
            {
                user = userClient.Create(new UserRequest
                {
                    Name = name
                });
            }

            return user;
        }

        public static ActorResponse GetOrCreateGroup(GroupClient groupClient, string name)
        {
            var groups = groupClient.Get(name);
            ActorResponse group;

            if (groups.Any())
            {
                group = groups.Single();
            }
            else
            {
                group = groupClient.Create(new GroupRequest
                {
                    Name = name
                });
            }

            return group;
        }

        public static GameResponse GetOrCreateGame(GameClient gameClient, string name)
        {
            var games = gameClient.Get(name);
            GameResponse game;

            if (games.Any())
            {
                game = games.Single();
            }
            else
            {
                game = gameClient.Create(new GameRequest
                {
                    Name = name
                });
            }

            return game;
        }

        public static void Login(SessionClient sessionClient, int gameId, AccountRequest accountRequest)
        {
            try
            {
                sessionClient.Login(gameId, accountRequest);
            }
            catch (Exception e)
            {
                sessionClient.CreateAndLogin(gameId, accountRequest);
            }
        }

        public static void Login(SessionClient sessionClient, AccountRequest accountRequest)
        {
            try
            {
                sessionClient.Login(accountRequest);
            }
            catch (Exception e)
            {
                sessionClient.CreateAndLogin(accountRequest);
            }
        }
    }
}
