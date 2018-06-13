using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.EntityFramework.Tests;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.Core.Tests
{
	/*
	// This is to debug slow setup speeds. It will not run in conjunction with the cire tests
	public class CoreTestFixtureTests
	{
		[Fact]
		public void SetupSpeedTest()
		{
			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => new CoreTestFixture(), 40 * 1000, 1);
        }
	}*/

    public class CoreTestFixture
    {
		// Must be divisible by GroupCount and (FriendCount + 1)
        public const int UserCount = 100;
        public const int GameCount = 10;
        public const int GroupCount = 10;
        public const int FriendCount = 9;
        public const int UserDataCount = 100000;
	    public const int GroupDataCount = 10000;

        private readonly List<Game> _sortedGames = new List<Game>(GameCount);
        private readonly List<User> _sortedUsers = new List<User>(UserCount);
        private readonly List<Group> _sortedGroups = new List<Group>(GroupCount);
        
        private readonly UserController _userController = ControllerLocator.UserController;
        private readonly GroupController _groupController = ControllerLocator.GroupController;
        private readonly GameController _gameController = ControllerLocator.GameController;
	    private readonly RelationshipController _relationshipController = ControllerLocator.RelationshipController;
	    private readonly GameDataController _gameDataController = ControllerLocator.GameDataController;

        public IReadOnlyList<Game> SortedGames => _sortedGames;
        public IReadOnlyList<User> SortedUsers => _sortedUsers;
        public IReadOnlyList<Group> SortedGroups => _sortedGroups;

        public CoreTestFixture()
        {
			ClearDatabaseFixture.Clear();
            PopulateData();
        }

	    private void PopulateData()
	    {
		    _sortedGames.Clear();
		    _sortedUsers.Clear();
		    _sortedGroups.Clear();

		    // Add users and games
		    using (var context = EntityFramework.Tests.ControllerLocator.ContextFactory.Create())
		    {
			    for (var i = 0; i < UserCount; i++)
			    {
				    _sortedUsers.Add(CreateUser($"{i + 1}", context));
			    }

			    for (var i = 0; i < GameCount; i++)
			    {
				    _sortedGames.Add(CreateGame($"{i + 1}", context));
			    }

			    context.SaveChanges();
		    }

		    _sortedUsers.Sort((a, b) => a.Id - b.Id);
		    _sortedGames.Sort((a, b) => a.Id - b.Id);

		    // Add groups
		    var groupAdmin = CreateUser("TestingGroupAdmin");
		    for (var i = 0; i < GroupCount; i++)
		    {
			    _sortedGroups.Add(CreateGroup($"{i + 1}", groupAdmin.Id));
		    }

		    _sortedGroups.Sort((a, b) => a.Id - b.Id);

		    using (var context = EntityFramework.Tests.ControllerLocator.ContextFactory.Create())
		    {
			    for (var userIndex = 0; userIndex < _sortedUsers.Count; userIndex++)
			    {
				    var windowSize = FriendCount + 1;
				    var indexInWindow = userIndex % windowSize;
				    var friendsToAddCount = windowSize - (indexInWindow + 1);

				    for (var friendIndexOffset = 1; friendIndexOffset <= friendsToAddCount; friendIndexOffset++)
				    {
					    var friendIndex = userIndex + friendIndexOffset;

					    CreateFriendship(_sortedUsers[userIndex].Id, _sortedUsers[friendIndex].Id, context);
				    }

				    CreateMembership(_sortedUsers[userIndex].Id, _sortedGroups[userIndex % GroupCount].Id, context);
			    }

			    context.SaveChanges();
		    }

		    // Add user data
		    GenerateGameDataForActor(_sortedUsers.Cast<Actor>().ToList(), UserDataCount);

		    GenerateGameDataForActor(_sortedGroups.Cast<Actor>().ToList(), GroupDataCount);
        }

		private void GenerateGameDataForActor(List<Actor> actors, int dataCount)
		{ 
	        var evaluationDataType = (EvaluationDataType[])Enum.GetValues(typeof(EvaluationDataType));

            var data = new List<EvaluationData>();
	        var dataPerActor = dataCount / (float)actors.Count;
	        for (var i = 0; i < dataCount; i++)
	        {
		        var actorIndex = (int)Math.Floor(i / dataPerActor);

				var dataType = evaluationDataType[i % (evaluationDataType.Length - 1)];
		        
		        data.Add(new EvaluationData
		        {
					Category = EvaluationDataCategory.GameData,
					EvaluationDataType = dataType,
					ActorId = actors[actorIndex].Id,
					Key = $"{nameof(CoreTestFixture)}_generated",
					Value = $"{i}"
		        });
	        }

			_gameDataController.Add(data);
		}

		private User CreateUser(string name, SUGARContext context = null)
        {
            var user = new User
            {
                Name = $"User_{name}",
            };
            _userController.Create(user, context);

            return user;
        }

        private Group CreateGroup(string name, int creatorId)
        {
            var group = new Group
            {
                Name = $"Group_{name}",
            };
            
            _groupController.Create(group, creatorId);

            return group;
        }

        private Game CreateGame(string name, SUGARContext context)
        {
            var game = new Game
            {
                Name = $"Game_{name}",
            };
            //todo use actual user id instead of 0
            _gameController.Create(game, 1, context);

            return game;
        }

        private void CreateFriendship(int requestor, int acceptor, SUGARContext context = null)
        {
            var relationship = new ActorRelationship
			{
                RequestorId = requestor,
                AcceptorId = acceptor,
            };
	        _relationshipController.CreateRequest(relationship, true, context);
        }

        private void CreateMembership(int requestor, int acceptor, SUGARContext context)
        {
            var relationship = new ActorRelationship
            {
                RequestorId = requestor,
                AcceptorId = acceptor,
            };
	        _relationshipController.CreateRequest(relationship, true, context);
        }
    }

	[CollectionDefinition(nameof(CoreTestFixtureCollection))]
	public class CoreTestFixtureCollection : ICollectionFixture<CoreTestFixture>
	{
		// This class has no code, and is never created. Its purpose is simply
		// to be the place to apply [CollectionDefinition] and all the
		// ICollectionFixture<> interfaces.
	}
}
