using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserAchievementController : Controller
    {
        // POST api/groupachievement/sharing/1/...
        [HttpPost("{name}/{gameId}/{completionCriteria}")]
        public void Create(int gameId, string name, string completionCriteria)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/gameId/name
        [HttpGet("{name}/{gameId}")]
        public IEnumerable<Achievement> Get(string name, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/achievementId
        [HttpDelete("{achievmentId}")]
        public void Delete(int achievementId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
        public IEnumerable<AchievementProgress> GetProgress(int actorId, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupachievement/{2, 4, 7}/3
        [HttpGet("{actorIds}/progress/{achievemetnId}")]
        public IEnumerable<AchievementProgress> GetProgress(List<int> actorIds, int achievementId)
        {
            throw new NotImplementedException();
        }
    }
}