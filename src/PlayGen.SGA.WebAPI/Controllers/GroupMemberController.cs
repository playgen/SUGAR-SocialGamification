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
    public class GroupMemberController : Controller, IGroupMemberController
    {
        private readonly GroupMemberDbController _groupMemberDbController;

        public GroupMemberController(GroupMemberDbController groupMemberDbController)
        {
            _groupMemberDbController = groupMemberDbController;
        }

        // POST api/groupmember
        [HttpPost]
        public void CreateMemberRequest([FromBody]Relationship relationship)
        {
            var request = _groupMemberDbController.Create(relationship.ToGroupModel());
            Relationship relation = new Relationship
            {
                RequestorId = request.RequestorId,
                AcceptorId = request.AcceptorId
            };
            _groupMemberDbController.UpdateRequest(relation.ToGroupModel(), true);
        }

        // GET api/groupmember/requests?groupId=1
        [HttpGet("requests")]
        public IEnumerable<Actor> GetMemberRequests(int groupId)
        {
            var actor = _groupMemberDbController.GetRequests(groupId);
            return actor.ToContract();
        }

        // PUT api/groupmember/request
        [HttpPut("request")]
        public void UpdateMemberRequest([FromBody] Relationship relationship)
        {
            _groupMemberDbController.UpdateRequest(relationship.ToGroupModel(), relationship.Accepted);
        }

        // GET api/groupmember/members?groupId=1
        [HttpGet("members")]
        public IEnumerable<Actor> GetMembers(int groupId)
        {
            var actor = _groupMemberDbController.GetMembers(groupId);
            return actor.ToContract();
        }

        // PUT api/groupmember/
        [HttpPut]
        public void UpdateMember([FromBody] Relationship relationship)
        {
            _groupMemberDbController.Update(relationship.ToGroupModel());
        }
    }
}