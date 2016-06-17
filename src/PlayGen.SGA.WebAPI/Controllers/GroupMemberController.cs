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

        // GET api/groupmember/requests?groupId=1
        [HttpGet("requests")]
        public IEnumerable<ActorResponse> GetMemberRequests(int groupId)
        {
            var actor = _groupMemberDbController.GetRequests(groupId);
            return actor.ToContract();
        }

        // GET api/groupmember/members?groupId=1
        [HttpGet("members")]
        public IEnumerable<ActorResponse> GetMembers(int groupId)
        {
            var actor = _groupMemberDbController.GetMembers(groupId);
            return actor.ToContract();
        }

        // POST api/groupmember
        [HttpPost]
        public RelationshipResponse CreateMemberRequest([FromBody]RelationshipRequest relationship)
        {
            var request = _groupMemberDbController.Create(relationship.ToGroupModel());
            RelationshipRequest relation = new RelationshipRequest
            {
                RequestorId = request.RequestorId,
                AcceptorId = request.AcceptorId
            };
            _groupMemberDbController.UpdateRequest(relation.ToGroupModel(), true);
            return request.ToContract();
        }

        // PUT api/groupmember/request
        [HttpPut("request")]
        public void UpdateMemberRequest([FromBody] RelationshipStatusUpdate relationship)
        {
            var relation = new RelationshipRequest {
                RequestorId = relationship.RequestorId,
                AcceptorId = relationship.AcceptorId
            };
            _groupMemberDbController.UpdateRequest(relation.ToGroupModel(), relationship.Accepted);
        }

        // PUT api/groupmember/
        [HttpPut]
        public void UpdateMember([FromBody] RelationshipStatusUpdate relationship)
        {
            var relation = new RelationshipRequest
            {
                RequestorId = relationship.RequestorId,
                AcceptorId = relationship.AcceptorId
            };
            _groupMemberDbController.Update(relation.ToGroupModel());
        }
    }
}