using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.WebAPI.Exceptions;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates UserAchievement specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class UserAchievementController : Controller, IUserAchievementController
    {
        private readonly UserAchievementDbController _userAchievementDbController;

        public UserAchievementController(UserAchievementDbController userAchievementDbController)
        {
            _userAchievementDbController = userAchievementDbController;
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
            var achievement = _userAchievementDbController.Get(gameId);
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

        // GET api/userachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int userId, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/userachievement/3
        [HttpGet("{achievemetnId}/progress")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, [FromBody] List<int> userIds)
        {
            throw new NotImplementedException();
        }
    }
}