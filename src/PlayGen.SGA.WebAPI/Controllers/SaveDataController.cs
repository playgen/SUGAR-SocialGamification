using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public abstract class SaveDataController : Controller
    {
        public void Add(int actorId, int gameId, string key, string value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SaveData> Get(int actorId, int gameId, IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }
    }
}