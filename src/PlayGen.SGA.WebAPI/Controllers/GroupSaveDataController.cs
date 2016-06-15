using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.DataModel;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupSaveDataController : Controller, IGroupSaveDataController
    {
        private GroupSaveDataDbController _groupSaveDataDbController;

        public GroupSaveDataController(GroupSaveDataDbController groupSaveDataDbController)
        {
            _groupSaveDataDbController = groupSaveDataDbController;
        }

        public void Add(int id, [FromBody]Contracts.SaveData data)
        {
            _groupSaveDataDbController.Create(id, data);
        }

        public IEnumerable<Contracts.SaveData> Get(int actorId, int gameId, IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }
    }
}
