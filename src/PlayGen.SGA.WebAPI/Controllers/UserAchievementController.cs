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
    public class UserAchievementController : Controller, IUserAchievementController
    {
        private readonly UserAchievementDbController _userAchievementDbController;

        public UserAchievementController(UserAchievementDbController userAchievementDbController)
        {
            _userAchievementDbController = userAchievementDbController;
        }

        // POST api/userachievement/
        [HttpPost]
        public void Create([FromBody] Achievement newAchievement)
        {
            _userAchievementDbController.Create(newAchievement.ToUserModel());
        }

        // GET api/userachievement/gameId/name
        [HttpGet]
        public IEnumerable<Achievement> Get(int[] gameIds)
        {
            //var achievement = _userAchievementDbController.Get(name, gameId);
            //return achievement.ToContract();
            return null;
        }

        // GET api/userchievement/id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userAchievementDbController.Delete(id);
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