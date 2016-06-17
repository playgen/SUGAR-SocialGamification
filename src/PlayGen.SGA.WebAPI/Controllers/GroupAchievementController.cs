using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupAchievementController : Controller, IGroupAchievementController
    {
        private readonly GroupAchievementDbController _groupAchievementDbController;

        public GroupAchievementController(GroupAchievementDbController groupAchievementDbController)
        {
            _groupAchievementDbController = groupAchievementDbController;
        }

        // GET api/groupachievement?gameId=1&gameId=2
        [HttpGet]
        public IEnumerable<AchievementResponse> Get(int[] gameId)
        {
            var achievement = _groupAchievementDbController.Get(gameId);
            return achievement.ToContract();
        }

        // POST api/groupachievement/
        [HttpPost]
        public AchievementResponse Create([FromBody] AchievementRequest newAchievement)
        {
            var achievement = _groupAchievementDbController.Create(newAchievement.ToGroupModel());
            return achievement.ToContract();
        }

        // DELETE api/groupachievement?id=1&id=2
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