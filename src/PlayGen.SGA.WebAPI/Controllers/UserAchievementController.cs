using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserAchievementController : Controller, IUserAchievementController
    {
        // POST api/userachievement/
        [HttpPost]
        public void Create([FromBody] Achievement achievement)
        {
            throw new NotImplementedException();
        }

        // GET api/userachievement/name/gameId
        [HttpGet("{name}/{gameId}")]
        public IEnumerable<Achievement> Get(string name, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/userchievement/achievementId
        [HttpDelete("{achievmentId}")]
        public void Delete(int achievementId)
        {
            throw new NotImplementedException();
        }

        // GET api/userachievement/2/3
        [HttpGet("{actorId}/progress/{gameId}")]
        public IEnumerable<AchievementProgress> GetProgress(int userId, int gameId)
        {
            throw new NotImplementedException();
        }

        // GET api/userachievement/3
        [HttpGet("{achievemetnId}/progress")]
        public IEnumerable<AchievementProgress> GetProgress(int achievementId, [FromBody] List<int> actorIds)
        {
            throw new NotImplementedException();
        }
    }
}