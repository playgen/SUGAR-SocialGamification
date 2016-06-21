using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SGA.DataController;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.WebAPI.ExtensionMethods;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.WebAPI.Exceptions;

namespace PlayGen.SGA.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates User to User relationship specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class UserFriendController : Controller, IUserFriendController
    {
        private readonly UserFriendDbController _userFriendDbController;

        public UserFriendController(UserFriendDbController userFriendDbController)
        {
            _userFriendDbController = userFriendDbController;
        }

        /// <summary>
        /// Get a list of all Users that have relationship requests for this <param name="userId"/>.
        /// 
        /// Example Usage: GET api/userfriend/requests?userId=1
        /// </summary>
        /// <param name="userId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        [HttpGet("requests")]
        public IEnumerable<ActorResponse> GetFriendRequests(int userId)
        {
            var actor = _userFriendDbController.GetRequests(userId);
            var actorContract = actor.ToContract();
            return actorContract;
        }

        /// <summary>
        /// Get a list of all Users that have relationships with this <param name="userId"/>.
        /// 
        /// Example Usage: GET api/userfriend/friends?userId=1
        /// </summary>
        /// <param name="userId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        [HttpGet("friends")]
        public IEnumerable<ActorResponse> GetFriends(int userId)
        {
            var actor = _userFriendDbController.GetFriends(userId);
            var actorContract = actor.ToContract();
            return actorContract;
        }

        /// <summary>
        /// Create a new relationship request between two Users.
        /// Requires a relationship between the two to not already exist.
        /// 
        /// Example Usage: POST api/userfriend
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
        /// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
        [HttpPost]
        public RelationshipResponse CreateFriendRequest([FromBody]RelationshipRequest relationship)
        {
            if (relationship == null)
            {
                throw new NullObjectException("Invalid object passed");
            }
            var request = relationship.ToGroupModel();
            if (relationship.AutoAccept)
            {
                _userFriendDbController.Create(relationship.ToUserModel());
            }
            else
            {
                _userFriendDbController.Create(relationship.ToUserModel());
            }
            var relationshipContract = request.ToContract();
            return relationshipContract;
        }

        /// <summary>
        /// Update an existing relationship request between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
        /// Requires the relationship request to already exist between the two Users.
        /// 
        /// Example Usage: PUT api/userfriend/request
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
        [HttpPut("request")]
        public void UpdateFriendRequest([FromBody] RelationshipStatusUpdate relationship)
        {
            var relation = new RelationshipRequest
            {
                RequestorId = relationship.RequestorId,
                AcceptorId = relationship.AcceptorId
            };
            _userFriendDbController.UpdateRequest(relation.ToUserModel(), relationship.Accepted);
        }

        /// <summary>
        /// Update an existing relationship between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
        /// Requires the relationship to already exist between the two Users.
        /// 
        /// Example Usage: PUT api/userfriend
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
        [HttpPut]
        public void UpdateFriend([FromBody] RelationshipStatusUpdate relationship)
        {
            var relation = new RelationshipRequest
            {
                RequestorId = relationship.RequestorId,
                AcceptorId = relationship.AcceptorId
            };
            _userFriendDbController.Update(relation.ToUserModel());
        }
    }
}