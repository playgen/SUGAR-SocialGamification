using System;
using System.Collections.Generic;

using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Extensions
{
	// ReSharper disable once InconsistentNaming
	public static class SUGARContextSeedExtensions
	{
		public static void Seed(this SUGARContext context)
		{
			var roles = new Dictionary<ClaimScope, Role>();

			foreach (var claimScope in (ClaimScope[])Enum.GetValues(typeof(ClaimScope)))
			{
				var addedClaimScope = CreateRole(context, claimScope);
				roles.Add(claimScope, addedClaimScope);
			}

			var adminUser = CreateUser(context, "admin");

			var adminAccount = CreateAccount(context, "admin", "$2a$12$SSIgQE0cQejeH0dM61JV/eScAiHwJo/I3Gg6xZFUc0gmwh0FnMFv.", CreateAccountSource(context, "SUGAR", "SUGAR", true), adminUser);

			#region Actor Roles
			//global (admin)
			CreateActorRole(context, roles[ClaimScope.Global], adminUser, Platform.EntityId);

			//global game control
			CreateActorRole(context, roles[ClaimScope.Game], adminUser, Platform.EntityId);

			//global group control
			CreateActorRole(context, roles[ClaimScope.Group], adminUser, Platform.EntityId);

			// admin user
			CreateActorRole(context, roles[ClaimScope.User], adminUser, adminUser.Id);

			//global user control
			CreateActorRole(context, roles[ClaimScope.User], adminUser, Platform.EntityId);

			// admin account
			CreateActorRole(context, roles[ClaimScope.Account], adminUser, adminAccount.Id);

			//global account control
			CreateActorRole(context, roles[ClaimScope.Account], adminUser, Platform.EntityId);

			//global role control
			CreateActorRole(context, roles[ClaimScope.Role], adminUser, Platform.EntityId);
			#endregion
		}

		public static void SeedTesting(this SUGARContext context)
		{
			var globalGame = CreateGame(context, "Global");
			#region Achievement Client Tests
			CreateAchievement(context, CreateGame(context, "Achievement_CanDisableNotifications").Id, "Achievement_CanDisableNotifications");
			CreateAchievement(context, CreateGame(context, "Achievement_CanGetNotifications").Id, "Achievement_CanGetNotifications");
			CreateAchievement(context, CreateGame(context, "Achievement_DontGetAlreadyRecievedNotifications").Id, "Achievement_DontGetAlreadyRecievedNotifications");
			CreateAchievement(context, globalGame.Id, "Achievement_CanGetGlobalAchievementProgress");
			CreateAchievement(context, CreateGame(context, "Achievement_CanGetAchievementProgress").Id, "Achievement_CanGetAchievementProgress");
			CreateGame(context, "Achievement_CannotGetNotExistingAchievementProgress");
			#endregion

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
			CreateGame(context, "GameData_CannotCreateWithMismatchedData");
			CreateGame(context, "GameData_CanGetGameData");
			CreateGame(context, "GameData_CannotGetGameDataWithoutActorId");
			CreateGame(context, "GameData_CanGetGameDataWithoutGameId");
			CreateGame(context, "GameData_CanGetGameDataByMultipleKeys");
			#endregion

			#region Skill Client Tests
			CreateSkill(context, CreateGame(context, "Skill_CanDisableNotifications").Id, "Skill_CanDisableNotifications");
			CreateSkill(context, CreateGame(context, "Skill_CanGetNotifications").Id, "Skill_CanGetNotifications");
			CreateSkill(context, CreateGame(context, "Skill_DontGetAlreadyRecievedNotifications").Id, "Skill_DontGetAlreadyRecievedNotifications");
			CreateSkill(context, globalGame.Id, "Skill_CanGetGlobalSkillProgress");
			CreateSkill(context, CreateGame(context, "Skill_CanGetSkillProgress").Id, "Skill_CanGetSkillProgress");
			CreateGame(context, "Skill_CannotGetNotExistingSkillProgress");
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
				Description = "SUGAR",
				Token = "SUGAR",
				RequiresPassword = true
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
				EvaluationCriterias = new List<Model.EvaluationCriteria>
				{
					new Model.EvaluationCriteria
					{
						ComparisonType = ComparisonType.GreaterOrEqual,
						CriteriaQueryType = CriteriaQueryType.Sum,
						EvaluationDataType = EvaluationDataType.Long,
						EvaluationDataKey = key,
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
				EvaluationCriterias = new List<Model.EvaluationCriteria>
				{
					new Model.EvaluationCriteria
					{
						ComparisonType = ComparisonType.GreaterOrEqual,
						CriteriaQueryType = CriteriaQueryType.Sum,
						EvaluationDataType = EvaluationDataType.Long,
						EvaluationDataKey = key,
						Value = $"{100}"
					}
				},
				Name = key,
				Token = key
			}).Entity;
			context.SaveChanges();
			return skill;
		}
	}
}
