using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using PlayGen.SGA.Contracts;

namespace PlayGen.SGA.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class GroupMemberController : Controller
    {
        // POST api/groupmember/1/request/2
        [HttpPost("{groupId}/request/{userId}")]
        public void CreateMemberRequest(int groupId, int userId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupmember/1/requests
        [HttpGet("{groupId}/requests")]
        public IEnumerable<Actor> GetMemberRequests(int groupId)
        {
            throw new NotImplementedException();
        }

        // GET api/groupmember/1/updatemember/2/true
        [HttpPost("{groupId}/request/{userId}/{accepted}")]
        public void UpdateMemberRequest(int groupId, int userId, bool accepted)
        {
            // TODO: remove request
            // If accepted, create membership
            throw new NotImplementedException();
        }

        // GET api/groupmember/1/member/2/accepted
        [HttpGet("{groupId}/members")]
        public IEnumerable<Actor> GetMembers(int groupId)
        {
            throw new NotImplementedException();
        }

        // PUT api/groupmember/1/member/2/accepted
        [HttpPatch("{groupId}/member/{userId}/{status}")]
        public void UpdateMember(int groupId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}