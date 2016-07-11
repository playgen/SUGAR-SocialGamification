using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.GameData;
using System;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates UserData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorization]
	public class ResourceController : Controller
	{
		private readonly GameData.ResourceController _resourceController;

		public ResourceController(GameData.ResourceController resourceController)
		{
			_resourceController = resourceController;
		}

		/// <summary>
		/// Find a list of all Resources filtered by the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/resource?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="keys">Optional array of Key names to filter results by.</param>
		/// <returns>A list of <see cref="ResourceResponse"/> which match the search criteria.</returns>
		[HttpGet]
		[ResponseType(typeof(IEnumerable<ResourceResponse>))]
		public IActionResult Get(int? gameId, int? actorId, string[] keys)
		{
			var resource = _resourceController.Get(gameId, actorId, keys.Any() ? keys : null);
			var resourceContract = resource.ToResourceContractList();
			return new ObjectResult(resourceContract);
		}

		/// <summary>
		/// Create a new Resource record.
		/// 
		/// Example Usage: POST api/resource
		/// </summary>
		/// <param name="resourceAddRequest"><see cref="ResourceAddRequest"/> object that holds the details of the new ResourceData.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the new Resource details.</returns>
		[HttpPost]
		[ResponseType(typeof(ResourceResponse))]
		[ArgumentsNotNull]
		public IActionResult Add([FromBody]ResourceAddRequest resourceAddRequest)
		{
			var resource = resourceAddRequest.ToModel();
			_resourceController.Create(resource);
			var resourceContract = resource.ToResourceContract();
			return new ObjectResult(resourceContract);
		}

		/// <summary>
		/// Update an existing Resource record.
		/// 
		/// Example Usage: Put api/resource/update?id=7
		/// </summary>
		/// <param name="id"></param>
		/// <param name="resourceUpdateRequest"><see cref="ResourceUpdateRequest"/> object that holds the details of the updated ResourceData.</param>
		[HttpPut("update/{id:int}")]
		[ArgumentsNotNull]
		public void Update([FromRoute] int id, [FromBody]ResourceUpdateRequest resourceUpdateRequest)
		{
			var resource = resourceUpdateRequest.ToModel();
			resource.Id = id;
			_resourceController.Update(resource);
		}

		/// <summary>
		/// Transfers a quantity of a specific resource.
		/// 
		/// Example Usage: Post api/resource/transfer
		/// </summary>
		/// <param name="transferRequest"><see cref="ResourceTransferRequest"/> object that holds the details of the resoruce transfer.</param>
		/// <returns>A <see cref="ResourceTransferResponse"/> containing the modified resources.</returns>
		[HttpPost("transfer")]
		[ResponseType(typeof(ResourceTransferResponse))]
		[ArgumentsNotNull]
		public IActionResult Transfer([FromBody] ResourceTransferRequest transferRequest)
		{
			Data.Model.GameData fromResource;

			var toResource = _resourceController.Transfer(transferRequest.ResourceId, 
				transferRequest.GameId, transferRequest.RecipientId, transferRequest.Quantity, out fromResource);

			var resourceTransferRespone = new ResourceTransferResponse
			{
				FromResource = fromResource.ToResourceContract(),
				ToResource = toResource.ToResourceContract(),
			};

			return new ObjectResult(resourceTransferRespone);
		}
	}
}
