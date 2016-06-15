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
        public int Create([FromBody] Achievement newAchievement)
        {
            var achievement = _userAchievementDbController.Create(newAchievement.ToUserModel());
            return achievement.Id;
        }

        // GET api/userachievement/gameId/name
        [HttpGet]
        public IEnumerable<Achievement> Get(int[] gameId)
        {
            var achievement = _userAchievementDbController.Get(gameId);
            return achievement.ToContract();
        }

        // GET api/userchievement/id
        [HttpDelete]
        public void Delete(int[] id)
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
        public IEnumerable<AchievementProgress> GetProgress(int achievementId, [FromBody] List<int> userIds)
        {
            throw new NotImplementedException();
        }
    }
}