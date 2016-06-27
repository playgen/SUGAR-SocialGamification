using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.WebAPI.ExtensionMethods;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Exceptions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates User specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		private readonly Data.EntityFramework.Controllers.UserController _userController;

		public UserController(Data.EntityFramework.Controllers.UserController userController)
		{
			_userController = userController;
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
			var user = _userController.Get();
			var actorContract = user.ToContract();
			return actorContract;
		}

		/// <summary>
		/// Get a list of Users that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/user/find/user1
		/// </summary>
		/// <param name="name">User name.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		public IEnumerable<ActorResponse> Get(string name)
		{
			var user = _userController.Search(name);
			var actorContract = user.ToContract();
			return actorContract;
		}

		/// <summary>
		/// Get User that matches <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/user/findbyid/1
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns><see cref="ActorResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}")]
		public ActorResponse Get(int id)
		{
			var user = _userController.Search(id);
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
			_userController.Create(user);
			var actorContract = user.ToContract();
			return actorContract;
		}

		/// <summary>
		/// Delete users with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/user?id=1&amp;id=2
		/// </summary>
		/// <param name="id">Array of User IDs.</param>
		[HttpDelete]
		public void Delete(int[] id)
		{
			_userController.Delete(id);
		}
	}
}