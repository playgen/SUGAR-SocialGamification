using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    public abstract class AchievementController : Controller
    {
        public void Create(string name, int gameId, string completionCriteria)
        {

            throw new NotImplementedException();
        }

        public IEnumerable<Achievement> Get(int gameId, string name)
        {
            throw new NotImplementedException();
        }

        public void Update(int achievementId)
        {
            throw new NotImplementedException();
        }

        public void Delete(int achievementId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AchievementProgress> GetProgress(int actorId, int gameId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AchievementProgress> GetProgress(List<int> actorIds, int achievementId)
        {
            throw new NotImplementedException();
        }
    }
}
