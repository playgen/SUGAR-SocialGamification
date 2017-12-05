using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
	/// Web Controller that facilitates UserData specific operations.
	/// </summary>
	/// // Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class ResourceController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.ResourceController _resourceController;

		public ResourceController(Core.Controllers.ResourceController resourceController, IAuthorizationService authorizationService)
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
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.User, AuthorizationAction.Get, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.Resource)]
		public async Task<IActionResult> Get(int? gameId, int? actorId, string[] keys)
		{
			if (gameId.HasValue && actorId.HasValue)
			{
				if ((await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, gameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
				{
					var resource = _resourceController.Get(gameId.Value, actorId.Value, keys.Any() ? keys : null);
					var resourceContract = resource.ToResourceContractList();
					return new ObjectResult(resourceContract);
				}
			}
			return Forbid();
		}

		/// <summary>
		/// Creates or updates a Resource record.
		/// 
		/// Example Usage: POST api/resource
		/// </summary>
		/// <param name="resourceRequest"><see cref="ResourceAddRequest"/> object that holds the details of the ResourceData.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the new Resource details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationAction.Create, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.User, AuthorizationAction.Create, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.Resource)]
		public async Task<IActionResult> AddOrUpdate([FromBody]ResourceAddRequest resourceRequest)
		{
			if ((await _authorizationService.AuthorizeAsync(User, resourceRequest.ActorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, resourceRequest.ActorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, resourceRequest.GameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var resource = resourceRequest.ToModel();
				var resources = _resourceController.Get(resourceRequest.GameId.Value, resourceRequest.ActorId.Value, new[] { resourceRequest.Key });
				if (resources.Any())
				{
					var firstResource = resources.Single();
					_resourceController.AddQuantity(firstResource.Id, resourceRequest.Quantity.Value);
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
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationAction.Update, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.User, AuthorizationAction.Update, AuthorizationEntity.Resource)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Update, AuthorizationEntity.Resource)]
		public async Task<IActionResult> Transfer([FromBody] ResourceTransferRequest transferRequest)
		{
			if ((await _authorizationService.AuthorizeAsync(User, transferRequest.SenderActorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, transferRequest.SenderActorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, transferRequest.GameId, HttpContext.ScopeItems(ClaimScope.Game))).Succeeded)
			{
				var toResource = _resourceController.Transfer(transferRequest.GameId.Value, transferRequest.SenderActorId.Value, transferRequest.RecipientActorId.Value, transferRequest.Key, transferRequest.Quantity.Value, out var fromResource);

				var resourceTransferRespone = new ResourceTransferResponse
				{
					FromResource = fromResource.ToResourceContract(),
					ToResource = toResource.ToResourceContract()
				};

				return new ObjectResult(resourceTransferRespone);
			}
			return Forbid();
		}
	}
}