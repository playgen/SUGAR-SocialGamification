using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.WebAPI.Extensions;

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
		//[ResponseType(typeof(IEnumerable<ResourceResponse>))]
		public IActionResult Get(int? gameId, int? actorId, string[] keys)
		{
			var resource = _resourceController.Get(gameId, actorId, keys.Any() ? keys : null);
			var resourceContract = resource.ToResourceContractList();
			return new ObjectResult(resourceContract);
		}

		/// <summary>
		/// Creates or updates a Resource record.
		/// 
		/// Example Usage: POST api/resource
		/// </summary>
		/// <param name="resourceRequest"><see cref="ResourceAddRequest"/> object that holds the details of the ResourceData.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the new Resource details.</returns>
		[HttpPost]
		//[ResponseType(typeof(ResourceResponse))]
		[ArgumentsNotNull]
		public IActionResult AddOrUpdate([FromBody]ResourceAddRequest resourceRequest)
		{
			var resource = resourceRequest.ToModel();
			var resources = _resourceController.Get(resourceRequest.GameId, resourceRequest.ActorId, new[] { resourceRequest.Key });
			if (resources.Any())
			{
				var firstResource = resources.ElementAt(0);
				_resourceController.UpdateQuantity(firstResource, resourceRequest.Quantity);
			}
			else
			{
				
				_resourceController.Create(resource);
				
			}

			var resourceContract = resource.ToResourceContract();
			return new ObjectResult(resourceContract);
		}

		/// <summary>
		/// Transfers a quantity of a specific resource.
		/// 
		/// Example Usage: Post api/resource/transfer
		/// </summary>
		/// <param name="transferRequest"><see cref="ResourceTransferRequest"/> object that holds the details of the resoruce transfer.</param>
		/// <returns>A <see cref="ResourceTransferResponse"/> containing the modified resources.</returns>
		[HttpPost("transfer")]
		//[ResponseType(typeof(ResourceTransferResponse))]
		[ArgumentsNotNull]
		public IActionResult Transfer([FromBody] ResourceTransferRequest transferRequest)
		{
			Data.Model.GameData fromResource;

			var toResource = _resourceController.Transfer(transferRequest.GameId, transferRequest.SenderActorId, transferRequest.RecipientActorId, transferRequest.Key, transferRequest.Quantity, out fromResource);

			var resourceTransferRespone = new ResourceTransferResponse
			{
				FromResource = fromResource.ToResourceContract(),
				ToResource = toResource.ToResourceContract(),
			};

			return new ObjectResult(resourceTransferRespone);
		}
	}
}