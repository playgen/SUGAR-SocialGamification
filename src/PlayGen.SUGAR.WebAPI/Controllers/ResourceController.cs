using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.WebAPI.Extensions;
using PlayGen.SUGAR.WebAPI.Filters;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates UserData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class ResourceController : Controller
	{
        private readonly IAuthorizationService _authorizationService;
        private readonly Core.Controllers.ResourceController _resourceController;

		public ResourceController(Core.Controllers.ResourceController resourceController,
                    IAuthorizationService authorizationService)
		{
			_resourceController = resourceController;
            _authorizationService = authorizationService;
        }

		/// <summary>
		/// Find a list of all Resources filtered by the <param name="actorId"/>, <param name="gameId"/> and <param name="keys"/> provided.
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
        [Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Resource)]
        public IActionResult AddOrUpdate([FromBody]ResourceAddRequest resourceRequest)
		{
            if (_authorizationService.AuthorizeAsync(User, resourceRequest.GameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var resource = resourceRequest.ToModel();
                var resources = _resourceController.Get(resourceRequest.GameId, resourceRequest.ActorId, new[] { resourceRequest.Key });
                var resourceList = resources as List<GameData> ?? resources.ToList();
                if (resourceList.Any())
                {
                    var firstResource = resourceList.ElementAt(0);
                    _resourceController.UpdateQuantity(firstResource, resourceRequest.Quantity);
                }
                else
                {

                    _resourceController.Create(resource);

                }

                var resourceContract = resource.ToResourceContract();
                return new ObjectResult(resourceContract);
            }
            return Unauthorized();
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
        [Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Resource)]
        public IActionResult Transfer([FromBody] ResourceTransferRequest transferRequest)
		{
            if (_authorizationService.AuthorizeAsync(User, transferRequest.GameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                GameData fromResource;

                var toResource = _resourceController.Transfer(transferRequest.GameId, transferRequest.SenderActorId, transferRequest.RecipientActorId, transferRequest.Key, transferRequest.Quantity, out fromResource);

                var resourceTransferRespone = new ResourceTransferResponse
                {
                    FromResource = fromResource.ToResourceContract(),
                    ToResource = toResource.ToResourceContract(),
                };

                return new ObjectResult(resourceTransferRespone);
            }
            return Unauthorized();
        }
	}
}