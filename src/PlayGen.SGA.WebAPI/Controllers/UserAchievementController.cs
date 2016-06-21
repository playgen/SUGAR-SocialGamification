using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
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
        /// GetByGame a list of UserAchievements that match <param name="gameId"/>.
        /// 
        /// Example Usage: GET api/userachievement?gameId=1&gameId=2
        /// </summary>
        /// <param name="gameId">Array of game IDs</param>
        /// <returns>Returns multiple <see cref="GameResponse"/> that hold UserAchievement details</returns>
        [HttpGet]
        public IEnumerable<AchievementResponse> Get(int[] gameId)
        {
            var achievement = _userAchievementDbController.GetByGame(gameId);
            return achievement.ToContract();
        }

        /// <summary>
        /// Create a new UserAchievement.
        /// Requires <see cref="newAchievement.Name"/> to be unique to that <see cref="newAchievement.GameId"/>.
        /// 
        /// Example Usage: POST api/userachievement/
        /// </summary>
        /// <param name="newAchievement"><see cref="AchievementRequest"/> object that holds the details of the new UserAchievement.</param>
        /// <returns>Returns a <see cref="AchievementResponse"/> object containing details for the newly created UserAchievement.</returns>
        [HttpPost]
        public AchievementResponse Create([FromBody] AchievementRequest newAchievement)
        {
            var achievement = _userAchievementDbController.Create(newAchievement.ToUserModel());
            return achievement.ToContract();
        }

        /// <summary>
        /// Delete UserAchievements with the <param name="id"/> provided.
        /// 
        /// Example Usage: DELETE api/userachievement?id=1&id=2
        /// </summary>
        /// <param name="id">Array of UserAchievement IDs</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _userAchievementDbController.Delete(id);
        }

        // GET api/userachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
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

        // GET api/userachievement/3
        [HttpGet("{achievemetnId}/progress")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, [FromBody] int[] userIds)
        {
            var achievementResponses = new List<AchievementProgressResponse>();
            var achievements = _userAchievementDbController.Get(new int[] {achievementId});

            if (!achievements.Any())
            {
                // TODO handle and notify - remove below
                throw new ArgumentOutOfRangeException();
            }

            var users = _userDbController.Get(userIds);

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
                    Actor = user.ToContract(),
                };

                achievementResponses.Add(achievementProgress);
            }

            return achievementResponses;
        }
    }
}