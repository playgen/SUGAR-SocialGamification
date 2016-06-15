using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserFriendController : Controller
    {
        // POST api/userfriend/1/request/2
        [HttpPost("{requestorId}/request/{acceptorId}")]
        public void CreateFriendRequest(int requestorId, int acceptorId)
        {
            throw new NotImplementedException();
        }

        // GET api/userfriend/1/requests
        [HttpGet("{userId}/requests")]
        public IEnumerable<Actor> GetFriendRequests(int userId)
        {
            throw new NotImplementedException();
        }

        // PUT api/userfriend/1/request
        [HttpPut("{acceptorId}/request")]
        public void UpdateFriendRequest(int acceptorId, [FromBody] Relationship relationship)
        {
            throw new NotImplementedException();
        }

        // GET api/userfriend/1/friends
        [HttpGet("{userId}/friends")]
        public IEnumerable<Actor> GetFriends(int userId)
        {
            throw new NotImplementedException();
        }

        // PUT api/userfriend/1
        [HttpPut("{userId}")]
        public void UpdateFriend(int userId, [FromBody] Relationship relationship)
        {
            throw new NotImplementedException();
        }
    }
}