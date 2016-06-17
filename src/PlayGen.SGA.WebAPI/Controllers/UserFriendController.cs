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
    public class UserFriendController : Controller, IUserFriendController
    {
        private readonly UserFriendDbController _userFriendDbController;

        public UserFriendController(UserFriendDbController userFriendDbController)
        {
            _userFriendDbController = userFriendDbController;
        }

        // GET api/userfriend/requests?userId=1
        [HttpGet("requests")]
        public IEnumerable<ActorResponse> GetFriendRequests(int userId)
        {
            var actor = _userFriendDbController.GetRequests(userId);
            return actor.ToContract();
        }

        // GET api/userfriend/friends?userId=1
        [HttpGet("friends")]
        public IEnumerable<ActorResponse> GetFriends(int userId)
        {
            var actor = _userFriendDbController.GetFriends(userId);
            return actor.ToContract();
        }

        // POST api/userfriend
        [HttpPost]
        public RelationshipResponse CreateFriendRequest([FromBody]RelationshipRequest relationship)
        {
            var relation = _userFriendDbController.Create(relationship.ToUserModel());
            return relation.ToContract();
        }


        // PUT api/userfriend/request
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

        // PUT api/userfriend/
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