using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates Group specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class GroupController : Controller, IGroupController
    {
        private GroupDbController _groupDbController;

        public GroupController(GroupDbController groupDbController)
        {
            _groupDbController = groupDbController;
        }

        /// <summary>
        /// GetByGame a list of all Groups.
        /// 
        /// Example Usage: GET api/group/all
        /// </summary>
        /// <returns>A list of <see cref="ActorResponse"/> that hold Group details.</returns>
        [HttpGet("all")]
        public IEnumerable<ActorResponse> Get()
        {
            var group = _groupDbController.Get();
            return group.ToContract();
        }

        /// <summary>
        /// GetByGame a list of Groups that match <param name="name"/> provided.
        /// 
        /// Example Usage: GET api/group?name=group1&name=group2
        /// </summary>
        /// <param name="name">Array of group names.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        [HttpGet]
        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var group = _groupDbController.Get(name);
            return group.ToContract();
        }

        /// <summary>
        /// Create a new Group.
        /// Requires the <see cref="ActorRequest.Name"/> to be unique for Groups.
        /// 
        /// Example Usage: POST api/group
        /// </summary>
        /// <param name="actor"><see cref="ActorRequest"/> object that holds the details of the new Group.</param>
        /// <returns>A <see cref="ActorResponse"/> containing the new Group details.</returns>
        [HttpPost]
        public ActorResponse Create([FromBody]ActorRequest actor)
        {
            var group = _groupDbController.Create(actor.ToGroupModel());
            return group.ToContract();
        }

        /// <summary>
        /// Delete groups with the <param name="id"/> provided.
        /// 
        /// Example Usage: DELETE api/group?id=1&id=2
        /// </summary>
        /// <param name="id">Array of Group IDs.</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _groupDbController.Delete(id);
        }
    }
}