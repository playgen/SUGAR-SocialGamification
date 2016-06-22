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
            var actorContract = user.ToContract();
            return actorContract;
        }

        /// <summary>
        /// Get a list of Users that match <param name="name"/> provided.
        /// 
        /// Example Usage: GET api/user?name=user1amp;name=user2
        /// </summary>
        /// <param name="name">Array of user names.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        [HttpGet]
        public IEnumerable<ActorResponse> Get(string[] name)
        {
            var user = _userDbController.Get(name);
            var actorContract = user.ToContract();
            return actorContract;
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
            if (actor == null)
            {
                throw new NullObjectException("Invalid object passed");
            }
            var user = actor.ToUserModel();
            _userDbController.Create(user);
            var actorContract = user.ToContract();
            return actorContract;
        }

        /// <summary>
        /// Delete users with the <param name="id"/> provided.
        /// 
        /// Example Usage: DELETE api/user?id=1amp;id=2
        /// </summary>
        /// <param name="id">Array of User IDs.</param>
        [HttpDelete]
        public void Delete(int[] id)
        {
            _userDbController.Delete(id);
        }
    }
}