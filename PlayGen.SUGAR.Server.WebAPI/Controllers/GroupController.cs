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
	/// Web Controller that facilitates Group specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class GroupController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.GroupController _groupCoreController;

		public GroupController(Core.Controllers.GroupController groupCoreController,
					IAuthorizationService authorizationService)
		{
			_groupCoreController = groupCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Get a list of all Groups.
		/// 
		/// Example Usage: GET api/group/list
		/// </summary>
		/// <returns>A list of <see cref="GroupResponse"/> that hold Group details.</returns>
		[HttpGet("list")]
		//[ResponseType(typeof(IEnumerable<GroupResponse>))]
		public IActionResult Get()
		{
			var groups = _groupCoreController.GetByPermissions(int.Parse(User.Identity.Name));
			var actorContract = groups.ToContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Get a list of Groups that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/group/find/group1
		/// </summary>
		/// <param name="name">Group name.</param>
		/// <returns>A list of <see cref="GroupResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		//[ResponseType(typeof(IEnumerable<GroupResponse>))]
		public IActionResult Get([FromRoute]string name)
		{
			var groups = _groupCoreController.Search(name);
			var actorContract = groups.ToContractList();

			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Get Group that matches <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/group/findbyid/1
		/// </summary>
		/// <param name="id">Group id.</param>
		/// <returns><see cref="GroupResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByGroupId")]
		//[ResponseType(typeof(GroupResponse))]
		public IActionResult Get([FromRoute]int id)
		{
			var group = _groupCoreController.Get(id);
			var actorContract = group.ToContract();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Create a new Group.
		/// Requires the <see cref="GroupRequest.Name"/> to be unique for Groups.
		/// 
		/// Example Usage: POST api/group
		/// </summary>
		/// <param name="actor"><see cref="GroupRequest"/> object that holds the details of the new Group.</param>
		/// <returns>A <see cref="GroupResponse"/> containing the new Group details.</returns>
		[HttpPost]
		//[ResponseType(typeof(GroupResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Create, AuthorizationEntity.Group)]
		public async Task<IActionResult> Create([FromBody]GroupRequest actor)
		{
			if (await _authorizationService.AuthorizeAsync(User, Platform.EntityId, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.Global)]))
			{
				var group = actor.ToGroupModel();
				_groupCoreController.Create(group, int.Parse(User.Identity.Name));
				var actorContract = group.ToContract();
				return new ObjectResult(actorContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Update an existing Group.
		/// 
		/// Example Usage: PUT api/group/update/1
		/// </summary>
		/// <param name="id">Id of the existing Group.</param>
		/// <param name="group"><see cref="GroupRequest"/> object that holds the details of the Group.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationAction.Update, AuthorizationEntity.Group)]
		// todo refactor to use groupupdaterequest that contains an Id property and have a separate groupcreaterequest that doen't have the Id
		public async Task<IActionResult> Update([FromRoute] int id, [FromBody] GroupRequest group)
		{
			if (await _authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.Group)]))
			{
				var groupModel = group.ToGroupModel();
				groupModel.Id = id;
				_groupCoreController.Update(groupModel);
				return Ok();
			}
			return Forbid();
		}

		/// <summary>
		/// Delete group with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/group/1
		/// </summary>
		/// <param name="id">Group ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Group, AuthorizationAction.Delete, AuthorizationEntity.Group)]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			if (await _authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.Group)]))
			{
				_groupCoreController.Delete(id);
				return Ok();
			}
			return Forbid();
		}
	}
}