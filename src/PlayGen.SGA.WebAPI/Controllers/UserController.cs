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
    /// Web Controller that facilitates User specific operations.
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : Controller, IUserController
    {
        private UserDbController _userDbController;

        public UserController(UserDbController userDbController)
        {
            _userDbController = userDbController;
        }

        /// <summary>
        /// Get a list of all Users.
        /// 
        /// Example Usage: GET api/user/all
        /// </summary>
        /// <returns>A list of <see cref="ActorResponse"/> that hold User details.</returns>
        [HttpGet("all")]
        public IEnumerable<ActorResponse> Get()
        {
            var user = _userDbController.Get();
            return user.ToContract();
        }

        /// <summary>
        /// Get a list of Users that match <param name="name"/> provided.
        /// 
        /// Example Usage: GET api/user?name=user1&name=user2
        /// </summary>
        /// <param name="name">Array of user names.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        [HttpGet]
        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var users = _userDbController.Get(name);
            return users.ToContract();
        }

        /// <summary>
        /// Create a new User.
        /// Requires the <see cref="ActorRequest.Name"/> to be unique for Users.
        /// 
        /// Example Usage: POST api/user
        /// </summary>
        /// <param name="actor"><see cref="ActorRequest"/> object that holds the details of the new User.</param>
        /// <returns>A <see cref="ActorResponse"/> containing the new User details.</returns>
        [HttpPost]
        public ActorResponse Create([FromBody]ActorRequest actor)
        {
            var user = actor.ToUserModel();
            _userDbController.Create(user);
            return user.ToContract();
        }

        /// <summary>
        /// Delete users with the <param name="id"/> provided.
        /// 
        /// Example Usage: DELETE api/user?id=1&id=2
        /// </summary>
        /// <param name="id">Array of User IDs.</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _userDbController.Delete(id);
        }
    }
}