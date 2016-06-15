using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupAchievementController : Controller, IGroupAchievementController
    {
        // POST api/groupachievement/
        [HttpPost]
        public void Create([FromBody] Achievement achievement)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/name/gameId
        [HttpGet("{name}/{gameId}")]
        public IEnumerable<Achievement> Get(string name, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupchievement/achievementId
        [HttpDelete("{achievmentId}")]
        public void Delete(int achievementId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
        public IEnumerable<AchievementProgress> GetProgress(int groupId, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/3
        [HttpGet("{achievementId}/progress")]
        public IEnumerable<AchievementProgress> GetProgress(int achievementId, [FromBody] List<int> actorIds)
        {
            throw new NotImplementedException();
        }
    }
}