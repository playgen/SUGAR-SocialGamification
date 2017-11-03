using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Authorization;
using PlayGen.SUGAR.Server.Model;
using PlayGen.SUGAR.Server.WebAPI.Attributes;
using PlayGen.SUGAR.Server.WebAPI.Extensions;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates UserData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
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
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.Resource)]
		public async Task<IActionResult> AddOrUpdate([FromBody]ResourceAddRequest resourceRequest)
		{
			if (await _authorizationService.AuthorizeAsync(User, resourceRequest.GameId, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.Game)]))
			{
				var resource = resourceRequest.ToModel();
				var resources = _resourceController.Get(resourceRequest.GameId, resourceRequest.ActorId, new[] { resourceRequest.Key });
				if (resources.Any())
				{
					var firstResource = resources.Single();
					_resourceController.AddQuantity(firstResource.Id, resourceRequest.Quantity);
				}
				else
				{

					_resourceController.Create(resource);

				}

				var resourceContract = resource.ToResourceContract();
				return new ObjectResult(resourceContract);
			}
			return Forbid();
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
		[Authorization(ClaimScope.Group, AuthorizationAction.Update, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.Resource)]
		public async Task<IActionResult> Transfer([FromBody] ResourceTransferRequest transferRequest)
		{
			if (await _authorizationService.AuthorizeAsync(User, transferRequest.SenderActorId, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.Group)]) ||
				await _authorizationService.AuthorizeAsync(User, transferRequest.SenderActorId, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.User)]))
			{
				var toResource = _resourceController.Transfer(transferRequest.GameId, transferRequest.SenderActorId, transferRequest.RecipientActorId, transferRequest.Key, transferRequest.Quantity, out var fromResource);

				var resourceTransferRespone = new ResourceTransferResponse
				{
					FromResource = fromResource.ToResourceContract(),
					ToResource = toResource.ToResourceContract(),
				};

				return new ObjectResult(resourceTransferRespone);
			}
			return Forbid();
		}

		/// <summary>
		/// Adds a quantity of a specific resource.
		/// 
		/// Example Usage: Post api/resource/add
		/// </summary>
		/// <param name="addRequest"><see cref="ResourceAddRequest"/> object that holds the details of the resoruce transfer.</param>
		/// <returns>A <see cref="ResourceAddResponse"/> containing the modified resources.</returns>
		[HttpPost("add")]
		//[ResponseType(typeof(ResourceTransferResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationAction.Update, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.Resource)]
		public async Task<IActionResult> AddResource([FromBody] ResourceAddRequest addRequest)
		{
			if (await _authorizationService.AuthorizeAsync(User, addRequest.ActorId, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.Group)]) ||
				await _authorizationService.AuthorizeAsync(User, addRequest.ActorId, (AuthorizationRequirement)HttpContext.Items[AuthorizationAttribute.Key(ClaimScope.User)]))
			{
				var toResource = _resourceController.AddResource(addRequest.GameId, addRequest.ActorId, addRequest.Key, addRequest.Quantity);
					
				var resourceAddResponse = new ResourceAddResponse {
					Resource = toResource.ToResourceContract(),
				};

				return new ObjectResult(resourceAddResponse);
			}
			return Forbid();
		}
	}
}