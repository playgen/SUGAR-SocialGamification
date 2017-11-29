using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class MatchClientTests : ClientTestBase
	{
		private AccountResponse _account;
		private GameResponse _game;
		
		[Fact]
		public void CanStart()
		{
			// Arrange
			LoginUserForGame();

			// Act
			var match = SUGARClient.Match.CreateAndStart();

			// Assert
			Assert.Equal(_game.Id, match.Game.Id);
			Assert.Equal(_account.User.Id, match.Creator.Id);
		}

		[Fact]
		public void CanEnd()
		{
			// Arrange
			LoginUserForGame();
			var match = SUGARClient.Match.CreateAndStart();

			// Act
			match = SUGARClient.Match.End(match.Id);

			// Assert
			Assert.NotEqual(match.Ended, null);
			Assert.True(match.Started < match.Ended);
		}

		[Fact]
		public void CanGetByTime()
		{
			// Arrange
			LoginUserForGame();
			var shouldntGet = StartAndEndMatches(10);

			var preTime = DateTime.UtcNow;

			var shouldGet = StartAndEndMatches(10);
			
			var postTime = DateTime.UtcNow;

			shouldntGet.AddRange(StartAndEndMatches(10));

			// Act
			var got = SUGARClient.Match.GetByTime(preTime, postTime);

			// Assert
			shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));
			shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));
		}

		[Fact]
		public void GetByGame()
		{
			// Arrange
			LoginUserForGame("GetByGame_ShouldntGet");
			var shouldntGet = StartMatches(10);

			LoginUserForGame("GetByGame_ShouldGet");
			var shouldGet = StartMatches(10);

			// Act
			var got = SUGARClient.Match.GetByGame(_game.Id);

			// Assert
			got.ForEach(m => Assert.Equal(_game.Id, m.Game.Id));
			shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));
			shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));
		}

		[Fact]
		public void GetByGameAndTime()
		{
			// Arrange
			LoginUserForGame("GetByGameAndTime_ShouldntGet");
			var shouldntGet = StartAndEndMatches(10);

			var pre = DateTime.UtcNow;

			shouldntGet.AddRange(StartAndEndMatches(10));

			LoginUserForGame("GetByGameAndTime_ShouldGet");
			var shouldGet = StartAndEndMatches(10);

			var post = DateTime.UtcNow;

			shouldntGet.AddRange(StartAndEndMatches(10));

			// Act
			var got = SUGARClient.Match.GetByGame(_game.Id, pre, post);

			// Assert
			got.ForEach(m => Assert.Equal(_game.Id, m.Game.Id));
			shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));
			shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));
		}

		[Fact]
		public void CanGetByCreator()
		{
			// Arrange
			LoginUserForGame("CanGetByCreator_ShouldntGet");
			var shouldntGet = StartMatches(10);

			LoginUserForGame("CanGetByCreator_ShouldGet");
			var shouldGet = StartMatches(10);

			// Act
			var got = SUGARClient.Match.GetByCreator(_account.User.Id);

			// Assert
			got.ForEach(m => Assert.Equal(_account.User.Id, m.Creator.Id));
			shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));
			shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));
		}

		[Fact]
		public void CanGetByCreatorAndTime()
		{
			// Arrange
			LoginUserForGame("CanGetByCreatorAndTime_ShouldntGet");
			var shouldntGet = StartAndEndMatches(10);

			var pre = DateTime.UtcNow;

			shouldntGet.AddRange(StartAndEndMatches(10));

			LoginUserForGame("CanGetByCreatorAndTime_ShouldGet");
			var shouldGet = StartAndEndMatches(10);

			var post = DateTime.UtcNow;

			shouldntGet.AddRange(StartAndEndMatches(10));

			// Act
			var got = SUGARClient.Match.GetByCreator(_account.User.Id, pre, post);

			// Assert
			got.ForEach(m => Assert.Equal(_account.User.Id, m.Creator.Id));
			shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));
			shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));
		}

		[Fact]
		public void CanGetByGameAndCreator()
		{
			// Arrange
			LoginUserForGame("CanGetByGameAndCreator_ShouldntGet");
			var shouldntGet = StartMatches(10);

			LoginUserForGame("CanGetByGameAndCreator_ShouldGet");
			var shouldGet = StartMatches(10);

			// Act
			var got = SUGARClient.Match.GetByGameAndCreator(_game.Id, _account.User.Id);

			// Assert
			got.ForEach(m => Assert.Equal(_game.Id, m.Game.Id));
			got.ForEach(m => Assert.Equal(_account.User.Id, m.Creator.Id));
			shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));
			shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));
		}

		[Fact]
		public void CanGetByGameAndCreatorAndTime()
		{
			// Arrange
			LoginUserForGame("CanGetByGameAndCreatorAndTime_ShouldntGet");
			var shouldntGet = StartAndEndMatches(10);

			var pre = DateTime.UtcNow;

			shouldntGet.AddRange(StartAndEndMatches(10));

			LoginUserForGame("CanGetByGameAndCreatorAndTime_ShouldGet");
			var shouldGet = StartAndEndMatches(10);

			var post = DateTime.UtcNow;

			shouldntGet.AddRange(StartAndEndMatches(10));

			// Act
			var got = SUGARClient.Match.GetByGameAndCreator(_game.Id, _account.User.Id, pre, post);

			// Assert
			got.ForEach(m => Assert.Equal(_game.Id, m.Game.Id));
			got.ForEach(m => Assert.Equal(_account.User.Id, m.Creator.Id));
			shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));
			shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));
		}

		[Fact]
		public void CanAddAndGetData()
		{
			// Arrange
			LoginUserForGame("CanAddAndGetData");

			var match = SUGARClient.Match.CreateAndStart();

			var datas = new List<EvaluationDataResponse>();

			// Act
			for (var i = 0; i < 10; i++)
			{
				datas.Add(SUGARClient.Match.AddData(new EvaluationDataRequest
				{
					MatchId = match.Id,
					GameId = _game.Id,
					CreatingActorId = _account.User.Id,
					EvaluationDataType = EvaluationDataType.Long,
					Key = "CanAddAndGetData",
					Value = i.ToString()
				}));
			}

			var got = SUGARClient.Match.GetData(match.Id);

			// Assert
			datas.ForEach(a => Assert.True(got.Any(g => g.GameId == a.GameId
				&& g.MatchId == a.MatchId
				&& g.CreatingActorId == a.CreatingActorId
				&& g.Key == a.Key
				&& g.Value == a.Value)));
		}

		#region Helpers
		private void LoginUserForGame(string key = "MatchTest")
		{
			_game = Helpers.GetGame(SUGARClient.Game, $"{nameof(MatchClientTests)}_{key}");

			try
			{
				_account = SUGARClient.Session.Login(_game.Id, new AccountRequest
				{
					Name = key,
					Password = key + "Password",
					SourceToken = "SUGAR"
				});
			}
			catch
			{
				_account = SUGARClient.Session.CreateAndLogin(_game.Id, new AccountRequest
				{
					Name = key,
					Password = key + "Password",
					SourceToken = "SUGAR"
				});
			}
		}

		private List<MatchResponse> StartMatches(int count)
		{
			var matches = new List<MatchResponse>();
			for (var i = 0; i < count; i++)
			{
				var match = SUGARClient.Match.CreateAndStart();
				matches.Add(match);
			}

			return matches;
		}

		private List<MatchResponse> StartAndEndMatches(int count)
		{
			var matches = StartMatches(count);
			matches = matches.Select(m => SUGARClient.Match.End(m.Id)).ToList();
			return matches;
		}

		#endregion
	}
}
