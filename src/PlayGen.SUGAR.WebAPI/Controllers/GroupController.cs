using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Contracts.Controllers;
using PlayGen.SUGAR.WebAPI.ExtensionMethods;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Exceptions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Group specific operations.
	/// </summary>
	[Route("api/[controller]")]
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
		/// Example Usage: GET api/group/all
		/// </summary>
		/// <returns>A list of <see cref="ActorResponse"/> that hold Group details.</returns>
		[HttpGet("list")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult Get()
		{
			var group = _groupController.Get();
			var actorContract = group.ToContract();
			return Ok(actorContract);
		}

		/// <summary>
		/// Get a list of Groups that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/group/find/group1
		/// </summary>
		/// <param name="name">Group name.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet("find/{name}")]
		[ResponseType(typeof(IEnumerable<ActorResponse>))]
		public IActionResult Get([FromRoute]string name)
		{
			var group = _groupController.Search(name);
			var actorContract = group.ToContract();
			return Ok(actorContract);
		}

		/// <summary>
		/// Get Group that matches <param name="id"/> provided.
		/// 
		/// Example Usage: GET api/group/findbyid/1
		/// </summary>
		/// <param name="id">Group id.</param>
		/// <returns><see cref="ActorResponse"/> which matches search criteria.</returns>
		[HttpGet("findbyid/{id:int}")]
		[ResponseType(typeof(ActorResponse))]
		public IActionResult Get([FromRoute]int id)
		{
			var group = _groupController.Search(id);
			var actorContract = group.ToContract();
			return Ok(actorContract);
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
		[ResponseType(typeof(ActorResponse))]
		public IActionResult Create([FromBody]ActorRequest actor)
		{
			if (actor == null)
			{
				throw new NullObjectException("Invalid object passed");
			}
			var group = actor.ToGroupModel();
			_groupController.Create(group);
			var actorContract = group.ToContract();
			return Ok(actorContract);
		}

		/// <summary>
		/// Delete group with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/group/1
		/// </summary>
		/// <param name="id">Group ID.</param>
		[HttpDelete("{id:int}")]
		public IActionResult Delete([FromRoute]int id)
		{
			_groupController.Delete(id);
			return Ok();
		}
	}
}