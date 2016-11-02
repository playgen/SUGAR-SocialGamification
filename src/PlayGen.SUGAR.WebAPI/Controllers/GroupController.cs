using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Group specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
	public class GroupController : Controller
	{
		private readonly Data.EntityFramework.Controllers.GroupController _groupController;

		public GroupController(Data.EntityFramework.Controllers.GroupController groupController)
		{
			_groupController = groupController;
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
			var groups = _groupController.Get();
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
			var groups = _groupController.Search(name);
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
			var group = _groupController.Search(id);
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
		public IActionResult Create([FromBody]ActorRequest actor)
		{
			var group = actor.ToGroupModel();
			_groupController.Create(group);
			var actorContract = group.ToContract();
			return new ObjectResult(actorContract);
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
		public void Update([FromRoute] int id, [FromBody] ActorRequest group)
		{
			var groupModel = group.ToGroupModel();
			groupModel.Id = id;
			_groupController.Update(groupModel);
		}

		/// <summary>
		/// Delete group with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/group/1
		/// </summary>
		/// <param name="id">Group ID.</param>
		[HttpDelete("{id:int}")]
		public void Delete([FromRoute]int id)
		{
			_groupController.Delete(id);
		}
	}
}