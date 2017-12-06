using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates User specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class UserController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.UserController _userCoreController;

		public UserController(Core.Controllers.UserController userCoreController, IAuthorizationService authorizationService)
		{
			_userCoreController = userCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Get a list of all Users.
		/// 
		/// Example Usage: GET api/user/list
		/// </summary>
		/// <returns>A list of <see cref="UserResponse"/> that hold User details.</returns>
		[HttpGet("list")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Get, AuthorizationEntity.User)]
		public async Task<IActionResult> Get()
		{
			if ((await _authorizationService.AuthorizeAsync(User, Platform.AllId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var users = _userCoreController.Get();
				var actorContract = users.ToContractList();
				return new ObjectResult(actorContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of Users that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/user/find/user1
		/// </summary>
		/// <param name="name">User name.</param>
		/// <param name="exactMatch">Match the name exactly.</param>
		/// <returns>A list of <see cref="UserResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		[HttpGet("find/{name}/{exactMatch:bool}")]
		public IActionResult Get([FromRoute]string name, bool exactMatch = false)
		{
			var users = _userCoreController.Search(name, exactMatch);
			var actorContract = users.ToContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Get User that matches <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/user/findbyid/1
		/// </summary>
		/// <param name="id">User id.</param>
		/// <returns><see cref="UserResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByUserId")]
		//[ResponseType(typeof(UserResponse))]
		public IActionResult Get([FromRoute]int id)
		{
			var user = _userCoreController.Get(id);
			var actorContract = user.ToContract();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Create a new User.
		/// Requires the <see cref="UserRequest"/>'s Name to be unique for Users.
		/// 
		/// Example Usage: POST api/user
		/// </summary>
		/// <param name="actor"><see cref="UserRequest"/> object that holds the details of the new User.</param>
		/// <returns>A <see cref="UserResponse"/> containing the new User details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Create, AuthorizationEntity.User)]
		public async Task<IActionResult> Create([FromBody]UserRequest actor)
		{
			if ((await _authorizationService.AuthorizeAsync(User, Platform.AllId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				var user = actor.ToUserModel();
				_userCoreController.Create(user);
				var actorContract = user.ToContract();
				return new ObjectResult(actorContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Update an existing User.
		/// 
		/// Example Usage: PUT api/user/update/1
		/// </summary>
		/// <param name="id">Id of the existing User.</param>
		/// <param name="user"><see cref="UserRequest"/> object that holds the details of the User.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.User)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserRequest user)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var userModel = user.ToUserModel();
				userModel.Id = id;
				var updatedUserModel = _userCoreController.Update(userModel);
				return new ObjectResult(updatedUserModel.ToContract());
			}
			return Forbid();
		}

		/// <summary>
		/// Delete user with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/user/1
		/// </summary>
		/// <param name="id">User ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Delete, AuthorizationEntity.User)]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			if ((await _authorizationService.AuthorizeAsync(User, Platform.AllId, HttpContext.ScopeItems(ClaimScope.Global))).Succeeded)
			{
				_userCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}