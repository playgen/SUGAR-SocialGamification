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
    /// Web Controller that facilitates GroupAchievement specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class GroupAchievementController : Controller, IGroupAchievementController
    {
        private readonly GroupAchievementDbController _groupAchievementDbController;

        public GroupAchievementController(GroupAchievementDbController groupAchievementDbController)
        {
            _groupAchievementDbController = groupAchievementDbController;
        }

        /// <summary>
        /// Get a list of GroupAchievements that match <param name="gameId"/>.
        /// 
        /// Example Usage: GET api/groupachievement?gameId=1&gameId=2
        /// </summary>
        /// <param name="gameId">Array of game IDs</param>
        /// <returns>Returns multiple <see cref="GameResponse"/> that hold GroupAchievement details</returns>
        [HttpGet]
        public IEnumerable<AchievementResponse> Get(int[] gameId)
        {
            var achievement = _groupAchievementDbController.Get(gameId);
            var achievementContract = achievement.ToContract();
            return achievementContract;
        }

        /// <summary>
        /// Create a new GroupAchievement.
        /// Requires <see cref="newAchievement.Name"/> to be unique to that <see cref="newAchievement.GameId"/>.
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
            _groupAchievementDbController.Create(achievement);
            var achievementContract = achievement.ToContract();
            return achievementContract;
        }

        /// <summary>
        /// Delete GroupAchievements with the <param name="id"/> provided.
        /// 
        /// Example Usage: DELETE api/groupachievement?id=1&id=2
        /// </summary>
        /// <param name="id">Array of GroupAchievement IDs</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _groupAchievementDbController.Delete(id);
        }

        // GET api/groupachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int groupId, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/3
        [HttpGet("{achievemetnId}/progress")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, [FromBody] List<int> groupIds)
        {
            throw new NotImplementedException();
        }
    }
}