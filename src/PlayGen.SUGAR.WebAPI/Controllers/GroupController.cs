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
	/// Web Controller that facilitates Group specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class GroupController : Controller, IGroupController
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
		[HttpGet("all")]
		public IEnumerable<ActorResponse> Get()
		{
			var group = _groupController.Get();
			var actorContract = group.ToContract();
			return actorContract;
		}

		/// <summary>
		/// Get a list of Groups that match <param name="name"/> provided.
		/// 
		/// Example Usage: GET api/group?name=group1&amp;name=group2
		/// </summary>
		/// <param name="name">Array of group names.</param>
		/// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
		[HttpGet]
		public IEnumerable<ActorResponse> Get(string[] name)
		{
			var group = _groupController.Get(name);
			var actorContract = group.ToContract();
			return actorContract;
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
		public ActorResponse Create([FromBody]ActorRequest actor)
		{
			if (actor == null)
			{
				throw new NullObjectException("Invalid object passed");
			}
			var group = actor.ToGroupModel();
			_groupController.Create(group);
			var actorContract = group.ToContract();
			return actorContract;
		}

		/// <summary>
		/// Delete groups with the <param name="id"/> provided.
		/// 
		/// Example Usage: DELETE api/group?id=1&amp;id=2
		/// </summary>
		/// <param name="id">Array of Group IDs.</param>
		[HttpDelete]
		public void Delete(int[] id)
		{
			_groupController.Delete(id);
		}
	}
}