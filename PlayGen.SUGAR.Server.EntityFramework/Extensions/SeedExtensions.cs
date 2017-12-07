using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class SUGARContextSeedExtensions
	{
		public static void EnsureSeeded(this SUGARContext context)
		{
			var roles = new Dictionary<ClaimScope, Role>();

			foreach (var claimScope in (ClaimScope[])Enum.GetValues(typeof(ClaimScope)))
			{
				var addedClaimScope = CreateRole(context, claimScope);
				roles.Add(claimScope, addedClaimScope);
			}

			var adminUser = context.Users.FirstOrDefault(u => u.Name == "admin") ?? CreateUser(context, "admin");

			var adminAccountSource = context.AccountSources.FirstOrDefault(a => a.Token == "SUGAR") ?? CreateAccountSource(context, "SUGAR", "SUGAR", true);

			var adminAccount = CreateAccount(context, "admin", "$2a$12$SSIgQE0cQejeH0dM61JV/eScAiHwJo/I3Gg6xZFUc0gmwh0FnMFv.", adminAccountSource, adminUser);

			#region Actor Roles
			if (!context.ActorRoles.Any())
			{
				//global (admin)
				CreateActorRole(context, roles[ClaimScope.Global], adminUser, Platform.AllId);

				//global game control
				CreateActorRole(context, roles[ClaimScope.Game], adminUser, Platform.AllId);

				//global group control
				CreateActorRole(context, roles[ClaimScope.Group], adminUser, Platform.AllId);

				// admin user
				CreateActorRole(context, roles[ClaimScope.User], adminUser, adminUser.Id);

				//global user control
				CreateActorRole(context, roles[ClaimScope.User], adminUser, Platform.AllId);

				// admin account
				CreateActorRole(context, roles[ClaimScope.Account], adminUser, adminAccount.Id);

				//global account control
				CreateActorRole(context, roles[ClaimScope.Account], adminUser, Platform.AllId);

				//global role control
				CreateActorRole(context, roles[ClaimScope.Role], adminUser, Platform.AllId);
			}
			#endregion
		}

		public static void EnsureTestsSeeded(this SUGARContext context)
		{
			if (!context.Games.Any())
			{
				CreateGame(context, "Global");

				#region Game Client Tests
				CreateGame(context, "Game_CanGetGamesByName 1");
				CreateGame(context, "Game_CanGetGamesByName 2");
				CreateGame(context, "Game_CanGetGameById");
				#endregion

				#region GameData Client Tests
				CreateGame(context, "GameData_CanCreate");
				CreateGame(context, "GameData_CannotCreateWithoutActorId");
				CreateGame(context, "GameData_CannotCreateWithoutKey");
				CreateGame(context, "GameData_CannotCreateWithoutValue");
				CreateGame(context, "GameData_CannotCreateWithoutEvaluationDataType");
				CreateGame(context, "GameData_CannotCreateWithMismatchedData");
				CreateGame(context, "GameData_CanGetGameData");
				CreateGame(context, "GameData_CannotGetGameDataWithoutActorId");
				CreateGame(context, "GameData_CanGetGameDataWithoutGameId");
				CreateGame(context, "GameData_CanGetGameDataByMultipleKeys");
				#endregion

				#region Resource Client Tests
				CreateGame(context, "Resource_CanCreate");
				CreateGame(context, "Resource_CannotCreateWithoutActorId");
				CreateGame(context, "Resource_CanUpdateExisting");
				CreateGame(context, "Resource_CanGetResource");
				CreateGame(context, "Resource_CannotGetResourceWithoutActorId");
				CreateGame(context, "Resource_CanGetResourceByMultipleKeys");
				CreateGame(context, "Resource_CannotCreateWithoutQuantity");
				CreateGame(context, "Resource_CannotCreateWithoutKey");
				#endregion
			}

			#region Achievement Client Tests
			if (!context.Achievements.Any())
			{
				CreateAchievement(context, CreateGame(context, "Achievement_CanDisableNotifications").Id, "Achievement_CanDisableNotifications");
				CreateAchievement(context, CreateGame(context, "Achievement_CanGetNotifications").Id, "Achievement_CanGetNotifications");
				CreateAchievement(context, CreateGame(context, "Achievement_DontGetAlreadyRecievedNotifications").Id, "Achievement_DontGetAlreadyRecievedNotifications");
				CreateAchievement(context, 0, "Achievement_CanGetGlobalAchievementProgress");
				CreateAchievement(context, CreateGame(context, "Achievement_CanGetAchievementProgress").Id, "Achievement_CanGetAchievementProgress");
				CreateGame(context, "Achievement_CannotGetNotExistingAchievementProgress");
			}
			#endregion

			#region Leaderboard Client Tests
			if (!context.Leaderboards.Any())
			{
				var leaderboardCanGetLeaderboardsByGame = CreateGame(context, "Leaderboard_CanGetLeaderboardsByGame");
				CreateLeaderboard(context, leaderboardCanGetLeaderboardsByGame.Id, leaderboardCanGetLeaderboardsByGame.Name + "1");
				CreateLeaderboard(context, leaderboardCanGetLeaderboardsByGame.Id, leaderboardCanGetLeaderboardsByGame.Name + "2");
				CreateLeaderboard(context, leaderboardCanGetLeaderboardsByGame.Id, leaderboardCanGetLeaderboardsByGame.Name + "3");
				CreateGame(context, "Leaderboard_CannotGetLeaderboardWithEmptyToken");
				CreateLeaderboard(context, 0, "Leaderboard_CanGetGlobalLeaderboardStandings");
				var leaderboardCanGetLeaderboardStandings = CreateGame(context, "Leaderboard_CanGetLeaderboardStandings");
				CreateLeaderboard(context, leaderboardCanGetLeaderboardStandings.Id, leaderboardCanGetLeaderboardStandings.Name);
				CreateGame(context, "Leaderboard_CannotGetNotExistingLeaderboardStandings");
				var leaderboardCannotGetLeaderboardStandingsWithoutToken = CreateGame(context, "Leaderboard_CannotGetStandingsWithoutToken");
				CreateLeaderboard(context, leaderboardCannotGetLeaderboardStandingsWithoutToken.Id, leaderboardCannotGetLeaderboardStandingsWithoutToken.Name);
				var leaderboardCannotGetLeaderboardStandingsWithoutGameId = CreateGame(context, "Leaderboard_CannotGetStandingsWithoutGameId");
				CreateLeaderboard(context, leaderboardCannotGetLeaderboardStandingsWithoutGameId.Id, leaderboardCannotGetLeaderboardStandingsWithoutGameId.Name);
				var leaderboardCannotGetLeaderboardStandingsWithoutFilterType = CreateGame(context, "Leaderboard_CannotGetStandingsWithoutFilterType");
				CreateLeaderboard(context, leaderboardCannotGetLeaderboardStandingsWithoutFilterType.Id, leaderboardCannotGetLeaderboardStandingsWithoutFilterType.Name);
				var leaderboardCannotGetLeaderboardStandingsWithoutLimit = CreateGame(context, "Leaderboard_CannotGetStandingsWithoutLimit");
				CreateLeaderboard(context, leaderboardCannotGetLeaderboardStandingsWithoutLimit.Id, leaderboardCannotGetLeaderboardStandingsWithoutLimit.Name);
				var leaderboardCannotGetLeaderboardStandingsWithoutOffset = CreateGame(context, "Leaderboard_CannotGetStandingsWithoutOffset");
				CreateLeaderboard(context, leaderboardCannotGetLeaderboardStandingsWithoutOffset.Id, leaderboardCannotGetLeaderboardStandingsWithoutOffset.Name);
				var leaderboardCanGetMultipleLeaderboardStandingsForActor = CreateGame(context, "Leaderboard_CanGetMultipleLeaderboardStandingsForActor");
				CreateLeaderboard(context, leaderboardCanGetMultipleLeaderboardStandingsForActor.Id, leaderboardCanGetMultipleLeaderboardStandingsForActor.Name);
				var leaderboardCannotGetLeaderboardStandingsWithIncorrectActorType = CreateGame(context, "Leaderboard_CannotGetStandingsWithIncorrectActorType");
				CreateLeaderboard(context, leaderboardCannotGetLeaderboardStandingsWithIncorrectActorType.Id, leaderboardCannotGetLeaderboardStandingsWithIncorrectActorType.Name, ActorType.Group);
				var leaderboardCannotGetLeaderboardStandingsWithZeroPageLimit = CreateGame(context, "Leaderboard_CannotGetLeaderboardStandingsWithZeroPageLimit");
				CreateLeaderboard(context, leaderboardCannotGetLeaderboardStandingsWithZeroPageLimit.Id, leaderboardCannotGetLeaderboardStandingsWithZeroPageLimit.Name);
				var leaderboardCannotGetNearLeaderboardStandingsWithoutActorId = CreateGame(context, "Leaderboard_CannotGetNearLeaderboardStandingsWithoutActorId");
				CreateLeaderboard(context, leaderboardCannotGetNearLeaderboardStandingsWithoutActorId.Id, leaderboardCannotGetNearLeaderboardStandingsWithoutActorId.Name);
				var leaderboardCannotGetFriendsLeaderboardStandingsWithoutActorId = CreateGame(context, "Leaderboard_CannotGetFriendsLeaderboardStandingsWithoutActorId");
				CreateLeaderboard(context, leaderboardCannotGetFriendsLeaderboardStandingsWithoutActorId.Id, leaderboardCannotGetFriendsLeaderboardStandingsWithoutActorId.Name);
				var leaderboardCannotGetGroupMembersLeaderboardStandingsWithoutActorId = CreateGame(context, "Leaderboard_CannotGetGroupMemberWithoutActorId");
				CreateLeaderboard(context, leaderboardCannotGetGroupMembersLeaderboardStandingsWithoutActorId.Id, leaderboardCannotGetGroupMembersLeaderboardStandingsWithoutActorId.Name, ActorType.Group);
				var leaderboardCannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType = CreateGame(context, "Leaderboard_CannotGetGroupMembersWithIncorrectActorType");
				CreateLeaderboard(context, leaderboardCannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType.Id, leaderboardCannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType.Name);
			}
			#endregion

			#region Skill Client Tests
			if (!context.Skills.Any())
			{
				CreateSkill(context, CreateGame(context, "Skill_CanDisableNotifications").Id, "Skill_CanDisableNotifications");
				CreateSkill(context, CreateGame(context, "Skill_CanGetNotifications").Id, "Skill_CanGetNotifications");
				CreateSkill(context, CreateGame(context, "Skill_DontGetAlreadyRecievedNotifications").Id,
					"Skill_DontGetAlreadyRecievedNotifications");
				CreateSkill(context, 0, "Skill_CanGetGlobalSkillProgress");
				CreateSkill(context, CreateGame(context, "Skill_CanGetSkillProgress").Id, "Skill_CanGetSkillProgress");
				CreateGame(context, "Skill_CannotGetNotExistingSkillProgress");
			}

			#endregion
		}

		private static Role CreateRole(SUGARContext context, ClaimScope claimScope, bool defaultRole = true)
		{
			var role = context.Roles.Add(new Role
			{
				Name = claimScope.ToString(),
				ClaimScope = claimScope,
				Default = defaultRole
			}).Entity;
			context.SaveChanges();
			return role;
		}

		private static User CreateUser(SUGARContext context, string name)
		{
			var user = context.Users.Add(new User
			{
				Name = name
			}).Entity;
			context.SaveChanges();
			return user;
		}

		private static Account CreateAccount(SUGARContext context, string name, string password, AccountSource source, User user)
		{
			var account = context.Accounts.Add(new Account
			{
				Name = name,
				Password = password,
				AccountSource = source,
				User = user
			}).Entity;
			context.SaveChanges();
			return account;
		}

		private static AccountSource CreateAccountSource(SUGARContext context, string description, string token, bool requiresPass)
		{
			var accountSource = context.AccountSources.Add(new AccountSource
			{
				Description = description,
				Token = token,
				RequiresPassword = requiresPass
			}).Entity;
			context.SaveChanges();
			return accountSource;
		}

		private static ActorRole CreateActorRole(SUGARContext context, Role role, Actor actor, int entityId)
		{
			var actorRole = context.ActorRoles.Add(new ActorRole
			{
				Role = role,
				Actor = actor,
				EntityId = entityId
			}).Entity;
			context.SaveChanges();
			return actorRole;
		}

		private static Game CreateGame(SUGARContext context, string name)
		{
			var game = context.Games.Add(new Game
			{
				Name = name
			}).Entity;
			context.SaveChanges();
			return game;
		}

		private static Achievement CreateAchievement(SUGARContext context, int gameId, string key)
		{
			var achievement = context.Achievements.Add(new Achievement
			{
				GameId = gameId,
				ActorType = ActorType.User,
				Description = key,
				EvaluationCriterias = new List<EvaluationCriteria>
				{
					new EvaluationCriteria
					{
						ComparisonType = ComparisonType.GreaterOrEqual,
						CriteriaQueryType = CriteriaQueryType.Sum,
						EvaluationDataCategory = EvaluationDataCategory.GameData,
						EvaluationDataType = EvaluationDataType.Long,
						EvaluationDataKey = key,
						Scope = CriteriaScope.Actor,
						Value = $"{100}"
					}
				},
				Name = key,
				Token = key
			}).Entity;
			context.SaveChanges();
			return achievement;
		}

		private static Skill CreateSkill(SUGARContext context, int gameId, string key)
		{
			var skill = context.Skills.Add(new Skill
			{
				GameId = gameId,
				ActorType = ActorType.User,
				Description = key,
				EvaluationCriterias = new List<EvaluationCriteria>
				{
					new EvaluationCriteria
					{
						ComparisonType = ComparisonType.GreaterOrEqual,
						CriteriaQueryType = CriteriaQueryType.Sum,
						EvaluationDataCategory = EvaluationDataCategory.GameData,
						EvaluationDataType = EvaluationDataType.Long,
						EvaluationDataKey = key,
						Scope = CriteriaScope.Actor,
						Value = $"{100}"
					}
				},
				Name = key,
				Token = key
			}).Entity;
			context.SaveChanges();
			return skill;
		}

		private static Leaderboard CreateLeaderboard(SUGARContext context, int gameId, string key, ActorType type = ActorType.User)
		{
			var leaderboard = context.Leaderboards.Add(new Leaderboard
			{
				Token = key,
				GameId = gameId,
				Name = key,
				EvaluationDataKey = key,
				EvaluationDataCategory = EvaluationDataCategory.GameData,
				ActorType = type,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			}).Entity;
			context.SaveChanges();
			return leaderboard;
		}
	}
}
