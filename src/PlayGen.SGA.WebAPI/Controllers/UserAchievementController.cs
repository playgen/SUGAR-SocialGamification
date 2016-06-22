using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.WebAPI.Exceptions;
using PlayGen.SGA.AchievementProgress;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates UserAchievement specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class UserAchievementController : Controller
    {
        private readonly UserAchievementDbController _userAchievementDbController;
        private readonly UserDbController _userDbController;
        private readonly AchievementProgressController _achievementProgressController;

        public UserAchievementController(UserAchievementDbController userAchievementDbController, 
            UserDbController userDbController,
            UserSaveDataDbController userSaveDataDbController)
        {
            _userAchievementDbController = userAchievementDbController;
            _userDbController = userDbController;
            _achievementProgressController = new AchievementProgressController(userSaveDataDbController);
        }

        /// <summary>
        /// Get a list of UserAchievements that match <param name="gameId"/>.
        /// 
        /// Example Usage: GET api/userachievement?gameId=1amp;gameId=2
        /// </summary>
        /// <param name="gameId">Array of game IDs</param>
        /// <returns>Returns multiple <see cref="GameResponse"/> that hold UserAchievement details</returns>
        [HttpGet]
        public IEnumerable<AchievementResponse> Get(int[] gameId)
        {
            var achievement = _userAchievementDbController.GetByGame(gameId);
            var achievementContract = achievement.ToContract();
            return achievementContract;
        }

        /// <summary>
        /// Create a new UserAchievement.
        /// Requires <see cref="AchievementRequest.Name"/> to be unique to that <see cref="AchievementRequest.GameId"/>.
        /// 
        /// Example Usage: POST api/userachievement/
        /// </summary>
        /// <param name="newAchievement"><see cref="AchievementRequest"/> object that holds the details of the new UserAchievement.</param>
        /// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created UserAchievement.</returns>
        [HttpPost]
        public AchievementResponse Create([FromBody] AchievementRequest newAchievement)
        {
            if (newAchievement == null)
            {
                throw new NullObjectException("Invalid object passed");
            }
            var achievement = newAchievement.ToUserModel();
            _userAchievementDbController.Create(achievement);
            var achievementContract = achievement.ToContract();
            return achievementContract;
        }

        /// <summary>
        /// Delete UserAchievements with the <param name="id"/> provided.
        /// 
        /// Example Usage: DELETE api/userachievement?id=1amp;id=2
        /// </summary>
        /// <param name="id">Array of UserAchievement IDs</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _userAchievementDbController.Delete(id);
        }

        /// <summary>
        /// Get the current progress for all achievements for a <param name="gameId"/> for <param name="userId"/>.
        /// 
        /// Example Usage: GET api/userachievement/gameprogress?gameId=1amp;userId=1
        /// </summary>
        /// <param name="userId">ID of User</param>
        /// <param name="gameId">ID of Game</param>
        /// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current user progress toward achievement.</returns>
        [HttpGet("gameprogress")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int userId, int gameId)
        {
            var achievementResponses = new List<AchievementProgressResponse>();
            var achievements = _userAchievementDbController.GetByGame(new int[] {gameId});

            foreach (var achievement in achievements)
            {
                var completed = _achievementProgressController.GetProgress(gameId, 
                    userId, 
                    achievement.CompletionCriteriaCollection);

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
        /// Get the current progress for an <param name="achievementId"/> for <param name="userId"/>.
        /// 
        /// Example Usage: GET api/userachievement/progress?achievementId=1amp;userId=1amp;userId=2
        /// </summary>
        /// <param name="achievementId">ID of Achievement</param>
        /// <param name="userIds">Array of User IDs</param>
        /// <returns>Returns multiple <see cref="AchievementProgressResponse"/> that hold current user progress toward achievement.</returns>
        [HttpGet("progress")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, int[] userId)
        {
            var achievementResponses = new List<AchievementProgressResponse>();
            var achievements = _userAchievementDbController.Get(new int[] {achievementId});

            if (!achievements.Any())
            {
                // TODO handle and notify - remove below
                throw new ArgumentOutOfRangeException();
            }

            var users = _userDbController.Get(userId);

            if (!users.Any())
            {
                // TODO handle and notify - remove below
                throw new ArgumentOutOfRangeException();
            }

            var achievement = achievements.ElementAt(0);

            foreach (var user in users)
            {
                var completed = _achievementProgressController.GetProgress(achievement.GameId, 
                    user.Id, 
                    achievement.CompletionCriteriaCollection);

                var achievementProgress = new AchievementProgressResponse
                {
                    Name = achievement.Name,
                    Progress = completed ? 1 : 0,
                    Actor = user.ToContract()
                };

                achievementResponses.Add(achievementProgress);
            }

            return achievementResponses;
        }
    }
}