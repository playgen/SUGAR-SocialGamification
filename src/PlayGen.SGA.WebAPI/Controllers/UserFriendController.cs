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

        // POST api/userfriend
        [HttpPost]
        public void CreateFriendRequest([FromBody]Relationship relationship)
        {
            _userFriendDbController.Create(relationship.ToUserModel());
        }

        // GET api/userfriend/requests?userId=1
        [HttpGet("requests")]
        public IEnumerable<Actor> GetFriendRequests(int userId)
        {
            var actor = _userFriendDbController.GetRequests(userId);
            return actor.ToContract();
        }

        // PUT api/userfriend/request
        [HttpPut("request")]
        public void UpdateFriendRequest([FromBody] Relationship relationship)
        {
            _userFriendDbController.UpdateRequest(relationship.ToUserModel(), relationship.Accepted);
        }

        // GET api/userfriend/friends?userId=1
        [HttpGet("friends")]
        public IEnumerable<Actor> GetFriends(int userId)
        {
            var actor = _userFriendDbController.GetFriends(userId);
            return actor.ToContract();
        }

        // PUT api/userfriend/
        [HttpPut]
        public void UpdateFriend([FromBody] Relationship relationship)
        {
            _userFriendDbController.Update(relationship.ToUserModel());
        }
    }
}