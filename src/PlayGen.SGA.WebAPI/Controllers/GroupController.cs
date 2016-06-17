using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;

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

        // POST api/group
        [HttpPost]
        public ActorResponse Create([FromBody]ActorRequest actor)
        {
            var group = _groupDbController.Create(actor.ToGroupModel());
            return group.ToContract();
        }

        // GET api/group/all
        [HttpGet("all")]
        public IEnumerable<ActorResponse> Get()
        {
            var group = _groupDbController.Get();
            return group.ToContract();
        }

        // GET api/group?name=group1&name=group2
        [HttpGet]
        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var group = _groupDbController.Get(name);
            return group.ToContract();
        }

        // DELETE api/group?id=1&id=2
        [HttpDelete]
        public void Delete(int[] id)
        {
            _groupDbController.Delete(id);
        }
    }
}