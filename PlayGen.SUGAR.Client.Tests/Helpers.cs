using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.Tests
{
	public static class Helpers
	{
		public static GameResponse GetGame(GameClient gameClient, string name)
		{
			var games = gameClient.Get(name);

			if (games.Any())
			{
				return games.Single();
			}
			throw new Exception("This game has not been set up in seeding.");
		}

		public static void Login(SUGARClient client, string gameName, string userKey, out GameResponse game, out AccountResponse user)
		{
			var accountRequest = new AccountRequest
			{
				Name = userKey,
				Password = "ThisIsTheTestingPassword",
				SourceToken = "SUGAR"
			};
			//gameId is 1 so that user is able to log in to get actual gameId (can in theory be anything)
			try
			{
				client.Session.Login(1, accountRequest);
			}
			catch
			{
				client.Session.CreateAndLogin(1, accountRequest);
			}
			var games = client.Game.Get(gameName);

			if (games.Any())
			{
				game = games.Single(g => g.Name == gameName);
				user = client.Session.Login(game.Id, accountRequest);
			}
			else
			{
				throw new Exception("This game has not been set up in seeding.");
			}
		}
	}
}
