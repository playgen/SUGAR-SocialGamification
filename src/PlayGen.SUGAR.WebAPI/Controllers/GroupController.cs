using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Group specific operations.
	/// </summary>
	[Route("api/[controller]")]
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
		/// <returns>A list of <see cref="ActorResponse"/> that hold Group details.</returns>
		[HttpGet("list")]
		//[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult Get()
		{
			var groups = _groupCoreController.Get();
			var actorContract = groups.ToContractList();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Get a list of Groups that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/group/find/group1
		/// </summary>
		/// <param name="name">Group name.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		//[ResponseType(typeof(IEnumerable<ActorResponse>))]
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
		/// <returns><see cref="ActorResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}", Name = "GetByGroupId")]
		//[ResponseType(typeof(ActorResponse))]
		public IActionResult Get([FromRoute]int id)
		{
			var group = _groupCoreController.Get(id);
			var actorContract = group.ToContract();
			return new ObjectResult(actorContract);
		}

		/// <summary>
		/// Create a new Group.
		/// Requires the <see cref="ActorRequest.Name"/> to be unique for Groups.
		/// 
		/// Example Usage: POST api/group
		/// </summary>
		/// <param name="actor"><see cref="ActorRequest"/> object that holds the details of the new Group.</param>
		/// <returns>A <see cref="ActorResponse"/> containing the new Group details.</returns>
		[HttpPost]
		//[ResponseType(typeof(ActorResponse))]
		[ArgumentsNotNull]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Create, AuthorizationOperation.Group)]
        public IActionResult Create([FromBody]ActorRequest actor)
		{
            if (_authorizationService.AuthorizeAsync(User, 0, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var group = actor.ToGroupModel();
                _groupCoreController.Create(group);
                var actorContract = group.ToContract();
                return new ObjectResult(actorContract);
            }
            return Unauthorized();
        }

		/// <summary>
		/// Update an existing Group.
		/// 
		/// Example Usage: PUT api/group/update/1
		/// </summary>
		/// <param name="id">Id of the existing Group.</param>
		/// <param name="group"><see cref="ActorRequest"/> object that holds the details of the Group.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
        [Authorization(ClaimScope.Actor, AuthorizationOperation.Update, AuthorizationOperation.Group)]
        // todo refactor to use groupupdaterequest that contains an Id property and have a separate groupcreaterequest that doen't have the Id
        public void Update([FromRoute] int id, [FromBody] ActorRequest group)
		{
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var groupModel = group.ToGroupModel();
                groupModel.Id = id;
                _groupCoreController.Update(groupModel);
            }
		}

		/// <summary>
		/// Delete group with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/group/1
		/// </summary>
		/// <param name="id">Group ID.</param>
		[HttpDelete("{id:int}")]
        [Authorization(ClaimScope.Actor, AuthorizationOperation.Delete, AuthorizationOperation.Group)]
        public void Delete([FromRoute]int id)
		{
            if (_authorizationService.AuthorizeAsync(User, id, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                _groupCoreController.Delete(id);
            }
		}
	}
}