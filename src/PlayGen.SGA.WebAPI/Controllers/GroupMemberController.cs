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
    /// Web Controller that facilitates User to Group relationship specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class GroupMemberController : Controller, IGroupMemberController
    {
        private readonly GroupMemberDbController _groupMemberDbController;

        public GroupMemberController(GroupMemberDbController groupMemberDbController)
        {
            _groupMemberDbController = groupMemberDbController;
        }

        /// <summary>
        /// Get a list of all Users that have relationship requests for this <param name="groupId"/>.
        /// 
        /// Example Usage: GET api/groupmember/requests?groupId=1
        /// </summary>
        /// <param name="groupId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        [HttpGet("requests")]
        public IEnumerable<ActorResponse> GetMemberRequests(int groupId)
        {
            var actor = _groupMemberDbController.GetRequests(groupId);
            return actor.ToContract();
        }

        /// <summary>
        /// Get a list of all Users that have relationships with this <param name="groupId"/>.
        /// 
        /// Example Usage: GET api/groupmember/members?groupId=1
        /// </summary>
        /// <param name="groupId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        [HttpGet("members")]
        public IEnumerable<ActorResponse> GetMembers(int groupId)
        {
            var actor = _groupMemberDbController.GetMembers(groupId);
            return actor.ToContract();
        }

        /// <summary>
        /// Create a new relationship request between the User and Group.
        /// Requires a relationship between the User and Group to not already exist.
        /// 
        /// Example Usage: POST api/groupmember
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
        /// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
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

        /// <summary>
        /// Update an existing relationship request between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
        /// Requires the relationship request to already exist between the User and Group.
        /// 
        /// Example Usage: PUT api/groupmember/request
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
        [HttpPut("request")]
        public void UpdateMemberRequest([FromBody] RelationshipStatusUpdate relationship)
        {
            var relation = new RelationshipRequest {
                RequestorId = relationship.RequestorId,
                AcceptorId = relationship.AcceptorId
            };
            _groupMemberDbController.UpdateRequest(relation.ToGroupModel(), relationship.Accepted);
        }

        /// <summary>
        /// Update an existing relationship between <param name="relationship.UserId"/> and <param name="relationship.GroupId"/>.
        /// Requires the relationship to already exist between the User and Group.
        /// 
        /// Example Usage: PUT api/groupmember
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
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