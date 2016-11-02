using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Web.Http.Description;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates User specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
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
		/// Example Usage: GET api/user/list
		/// </summary>
		/// <returns>A list of <see cref="ActorResponse"/> that hold User details.</returns>
		[HttpGet("list")]
		//[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult Get()
		{
			var users = _userController.Get();
			var actorContract = users.ToContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Get a list of Users that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/user/find/user1
		/// </summary>
		/// <param name="name">User name.</param>
		/// <param exactMatch="exactMatch">Match the name exactly.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		//[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult Get([FromRoute]string name, bool exactMatch)
		{
			var users = _userController.Search(name, exactMatch);
			var actorContract = users.ToContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Get User that matches <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/user/findbyid/1
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns><see cref="ActorResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByUserId")]
		//[ResponseType(typeof(ActorResponse))]
		public IActionResult Get([FromRoute]int id)
		{
			var user = _userController.Search(id);
			var actorContract = user.ToContract();
			return new ObjectResult(actorContract);
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
		//[ResponseType(typeof(ActorResponse))]
		[ArgumentsNotNull]
		public IActionResult Create([FromBody]ActorRequest actor)
		{
			var user = actor.ToUserModel();
			_userController.Create(user);
			var actorContract = user.ToContract();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Update an existing User.
		/// 
		/// Example Usage: PUT api/user/update/1
		/// </summary>
		/// <param name="id">Id of the existing User.</param>
		/// <param name="user"><see cref="ActorRequest"/> object that holds the details of the User.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		public void Update([FromRoute] int id, [FromBody] ActorRequest user)
		{
			var userModel = user.ToUserModel();
			userModel.Id = id;
			_userController.Update(userModel);
		}

		/// <summary>
		/// Delete user with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/user/1
		/// </summary>
		/// <param name="id">User ID.</param>
		[HttpDelete("{id:int}")]
		public void Delete([FromRoute]int id)
		{
			_userController.Delete(id);
		}
	}
}