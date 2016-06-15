using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupController : Controller, IGroupController
    {
        private GroupDbController _groupDbController;

        public GroupController(GroupDbController groupDbController)
        {
            _groupDbController = groupDbController;
        }

        // POST api/group/groupname
        [HttpPost("{name}")]
        public int Create(string name)
        {
            var group = _groupDbController.Create(name);
            return group.Id;
        }

        // GET api/group/groupname
        [HttpGet("{name}")]
        public Contracts.Actor Get(string name)
        {
            var group = _groupDbController.Get(name);
            return group.ToContract();
        }

        // DELETE api/group/1
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _groupDbController.Delete(id);
        }
    }
}