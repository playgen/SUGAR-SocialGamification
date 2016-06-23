using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.AchievementProgress;
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
        private readonly GroupDbController _groupDbController;
        private readonly AchievementProgressController _achievementProgressController;

        public GroupAchievementController(GroupAchievementDbController groupAchievementDbController,
            GroupDbController groupDbController,
            GroupSaveDataDbController groupSaveDataDbController)
        {
            _groupAchievementDbController = groupAchievementDbController;
            _groupDbController = groupDbController;
            _achievementProgressController = new AchievementProgressController(groupSaveDataDbController);
        }

        /// <summary>
        /// Get a list of GroupAchievements that match <param name="gameId"/>.
        /// 
        /// Example Usage: GET api/groupachievement?gameId=1&amp;gameId=2
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
            _groupAchievementDbController.Create(achievement);
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
            _groupAchievementDbController.Delete(id);
        }

        // GET api/groupachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int groupId, int gameId)
        {
            // TODO take a look at the implementation in UserAchievementController
            throw new NotImplementedException();
        }

        // GET api/groupachievement/3
        [HttpGet("{achievemetnId}/progress")]
        public IEnumerable<AchievementProgressResponse> GetProgress(int achievementId, int[] groupIds)
        {
            // TODO take a look at the implementation in UserAchievementController
            throw new NotImplementedException();
        }
    }
}