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

        // POST api/groupachievement/
        [HttpPost]
        public int Create([FromBody] Achievement newAchievement)
        {
            var achievement = _groupAchievementDbController.Create(newAchievement.ToGroupModel());
            return achievement.Id;
        }

        // GET api/groupachievement/gameId/name
        [HttpGet]
        public IEnumerable<Achievement> Get(int[] gameId)
        {
            var achievement = _groupAchievementDbController.Get(gameId);
            return achievement.ToContract();
        }

        // GET api/groupachievement/id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _groupAchievementDbController.Delete(id);
        }

        // GET api/groupachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
        public IEnumerable<AchievementProgress> GetProgress(int groupId, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/3
        [HttpGet("{achievemetnId}/progress")]
        public IEnumerable<AchievementProgress> GetProgress(int achievementId, [FromBody] List<int> groupIds)
        {
            throw new NotImplementedException();
        }
    }
}