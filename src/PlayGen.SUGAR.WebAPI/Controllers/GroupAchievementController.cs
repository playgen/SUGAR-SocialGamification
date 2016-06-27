using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.WebAPI.ExtensionMethods;
using PlayGen.SUGAR.WebAPI.Exceptions;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.GameData;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates GroupAchievement specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class GroupAchievementController : Controller
	{
		private readonly Data.EntityFramework.Controllers.GroupAchievementController _groupAchievementController;
		private readonly Data.EntityFramework.Controllers.GroupController _groupController;
		private readonly Data.EntityFramework.Controllers.GroupDataController _groupDataController;
		private readonly AchievementProgressController _achievementProgressController;

		public GroupAchievementController(Data.EntityFramework.Controllers.GroupAchievementController groupAchievementController,
			Data.EntityFramework.Controllers.GroupController groupController,
			Data.EntityFramework.Controllers.GroupDataController groupDataController)
		{
			_groupAchievementController = groupAchievementController;
			_groupController = groupController;
			_groupDataController = groupDataController;
			_achievementProgressController = new AchievementProgressController(groupDataController);
		}

		/// <summary>
		/// Get a list of GroupAchievements that match <param name="gameId"/>.
		/// 
		/// Example Usage: GET api/groupachievement?gameId=1
		/// </summary>
		/// <param name="gameId">Array of game IDs</param>
		/// <returns>Returns multiple <see cref="GameResponse"/> that hold GroupAchievement details</returns>
		[HttpGet]
		public IEnumerable<AchievementResponse> Get(int gameId)
		{
			var achievement = _groupAchievementController.GetByGame(gameId);
			var achievementContract = achievement.ToContract();
			return achievementContract;
		}

		/// <summary>
		/// Create a new GroupAchievement.
		/// Requires <see cref="AchievementRequest.Name"/> to be unique to that <see cref="AchievementRequest.GameId"/>.
		/// 
		/// Example Usage: POST api/groupachievement/
		/// </summary>
		/// <param name="newAchievement"><see cref="AchievementRequest"/> object that holds the details of the new GroupAchievement.</param>
		/// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created GroupAchievement.</returns>
		[HttpPost]
		public AchievementResponse Create([FromBody] AchievementRequest newAchievement)
		{
			if (newAchievement == null)
			{
				throw new NullObjectException("Invalid object passed");
			}
			var achievement = newAchievement.ToGroupModel();
			_groupAchievementController.Create(achievement);
			var achievementContract = achievement.ToContract();
			return achievementContract;
		}

		/// <summary>
		/// Delete GroupAchievements with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/groupachievement?id=1&amp;id=2
		/// </summary>
		/// <param name="id">Array of GroupAchievement IDs</param>
		[HttpDelete]
		public void Delete(int[] id)
		{
			_groupAchievementController.Delete(id);
		}

		/// <summary>
		/// Get the current progress for all achievements for a <param name="gameId"/> for <param name="groupId"/>.
		/// 
		/// Example Usage: GET api/groupachievement/gameprogress?gameId=1&amp;groupId=1
		/// </summary>
		/// <param name="groupId">ID of Group</param>
		/// <param name="gameId">ID of Game</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward achievement.</returns>
		[HttpGet("gameprogress")]
		public IEnumerable<AchievementProgressResponse> GetProgress(int groupId, int gameId)
		{
			var achievementResponses = new List<AchievementProgressResponse>();
			var achievements = _groupAchievementController.GetByGame(gameId);

			foreach (var achievement in achievements)
			{
				var completed = _achievementProgressController.IsAchievementCompleted(gameId,
					achievement.Id,
					groupId);
				if (!completed)
				{
					completed = _achievementProgressController.IsAchievementSatisified(gameId,
						groupId,
						achievement.CompletionCriteriaCollection);
					if (completed)
					{
						var groupData = new GroupData()
						{
							Key = $"GameId{gameId}AchievementId{achievement.Id}",
							GameId = gameId,
							GroupId = groupId,
							DataType = GameDataValueType.Boolean,
							Value = "true"
						};
						_groupDataController.Create(groupData);
					}
				}

				var achievementProgress = new AchievementProgressResponse
				{
					Name = achievement.Name,
					Progress = completed ? 1 : 0,
				};

				achievementResponses.Add(achievementProgress);
			}

			return achievementResponses;
		}

		/// <summary>
		/// Get the current progress for an <param name="achievementId"/> for <param name="groupId"/>.
		/// 
		/// Example Usage: GET api/groupachievement/progress?achievementId=1&amp;groupId=1&amp;groupId=2
		/// </summary>
		/// <param name="achievementId">ID of Achievement</param>
		/// <param name="groupId">Array of Group IDs</param>
		/// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current group progress toward achievement.</returns>
		[HttpGet("progress")]
		public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, int[] groupId)
		{
			var achievementResponses = new List<AchievementProgressResponse>();
			var achievements = _groupAchievementController.Get(new int[] { achievementId });

			if (!achievements.Any())
			{
				// TODO handle and notify - remove below
				throw new ArgumentOutOfRangeException();
			}

			var groups = groupId.Select(g => _groupController.Search(g));

			if (!groups.Any())
			{
				// TODO handle and notify - remove below
				throw new ArgumentOutOfRangeException();
			}

			var achievement = achievements.ElementAt(0);

			foreach (var group in groups)
			{
				var completed = _achievementProgressController.IsAchievementCompleted(achievement.GameId,
					achievement.Id,
					group.Id);
				if (!completed)
				{
					completed = _achievementProgressController.IsAchievementSatisified(achievement.GameId,
						group.Id,
						achievement.CompletionCriteriaCollection);
					if (completed)
					{
						var saveAchievement = new SaveDataRequest
						{
							Key = $"GameId{achievement.GameId}AchievementId{achievement.Id}",
							GameId = achievement.GameId,
							ActorId = group.Id,
							GameDataValueType = GameDataValueType.Boolean,
							Value = "true"
						};
						var saveAchievementModel = saveAchievement.ToGroupModel();
						_groupDataController.Create(saveAchievementModel);
					}
				}
				var achievementProgress = new AchievementProgressResponse
				{
					Name = achievement.Name,
					Progress = completed ? 1 : 0,
					Actor = group.ToContract()
				};

				achievementResponses.Add(achievementProgress);
			}

			return achievementResponses;
		}
	}
}