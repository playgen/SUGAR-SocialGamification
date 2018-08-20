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
	/// Web Controller that facilitates ActorClaim specific operations.
	/// </summary>
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	public class ActorClaimController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.ActorClaimController _actorClaimCoreController;
		private readonly Core.Authorization.ClaimController _claimCoreController;

		public ActorClaimController(Core.Controllers.ActorClaimController actorClaimCoreController, Core.Authorization.ClaimController claimCoreController, IAuthorizationService authorizationService)
		{
			_actorClaimCoreController = actorClaimCoreController;
			_claimCoreController = claimCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Get a list of all Actors for this Claim and Entity.
		/// </summary>
		/// <param name="claimId">Id of the Claim</param>
		/// <param name="entityId">Id of the Entity</param>
		/// <returns>A list of <see cref="ActorResponse"/> that holds Actor details.</returns>
		[HttpGet("claim/{claimId:int}/entity/{entityId:int}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Get, AuthorizationEntity.ActorClaim)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.ActorClaim)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Get, AuthorizationEntity.ActorClaim)]
		public async Task<IActionResult> GetClaimActors([FromRoute]int claimId, [FromRoute]int entityId)
		{
			var claim = _claimCoreController.Get(claimId);
			if (claim.ClaimScope == ClaimScope.Global)
			{
				entityId = Platform.AllId;
			}
			if ((await _authorizationService.AuthorizeAsync(User, entityId, HttpContext.ScopeItems(claim.ClaimScope))).Succeeded)
			{
				var actors = _actorClaimCoreController.GetClaimActors(claimId, entityId);
				var actorContract = actors.ToActorContractList();
				return new ObjectResult(actorContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Get a list of all Claims for this Actor.
		/// </summary>
		/// <param name="id">Id of the Actor</param>
		/// <returns>A list of <see cref="ActorClaimResponse"/> that holds ActorClaim details.</returns>
		[HttpGet("actor/{id:int}")]
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.ActorClaim)]
		[Authorization(ClaimScope.User, AuthorizationAction.Get, AuthorizationEntity.ActorClaim)]
		public async Task<IActionResult> GetActorClaims([FromRoute]int id)
		{
			if ((await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, id, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var claims = _actorClaimCoreController.GetActorClaims(id);
				var claimsContract = claims.ToContractList();
				return new ObjectResult(claimsContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new ActorClaim.
		/// </summary>
		/// <param name="newClaim"><see cref="ActorClaimRequest"/> object that contains the details of the new ActorClaim.</param>
		/// <returns>A <see cref="ActorClaimResponse"/> containing the new ActorClaim details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Global, AuthorizationAction.Create, AuthorizationEntity.ActorClaim)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Create, AuthorizationEntity.ActorClaim)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Create, AuthorizationEntity.ActorClaim)]
		public async Task<IActionResult> Create([FromBody]ActorClaimRequest newClaim)
		{
			var newClaimInfo = _claimCoreController.Get(newClaim.ClaimId.Value);
			if (newClaimInfo.ClaimScope == ClaimScope.Global)
			{
				newClaim.EntityId = Platform.AllId;
			}
			if ((await _authorizationService.AuthorizeAsync(User, newClaim.EntityId, HttpContext.ScopeItems(newClaimInfo.ClaimScope))).Succeeded)
			{
				var claimScope = _claimCoreController.Get(newClaim.ClaimId.Value).ClaimScope;
				var creatorClaims = _actorClaimCoreController.GetActorClaimsForEntity(int.Parse(User.Identity.Name), newClaim.EntityId.Value, claimScope);
				if (creatorClaims.Select(cc => cc.Id).Contains(newClaim.ClaimId.Value))
				{
					var claim = newClaim.ToModel();
					_actorClaimCoreController.Create(claim);
					var claimContract = claim.ToContract();
					return new ObjectResult(claimContract);
				}
			}
			return Forbid();
		}

		/// <summary>
		/// Delete ActorClaim with the ID provided.
		/// </summary>
		/// <param name="id">ActorClaim ID.</param>
		[HttpDelete("{id:int}")]
		[Authorization(ClaimScope.Global, AuthorizationAction.Delete, AuthorizationEntity.ActorClaim)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Delete, AuthorizationEntity.ActorClaim)]
		[Authorization(ClaimScope.Game, AuthorizationAction.Delete, AuthorizationEntity.ActorClaim)]
		public async Task<IActionResult> Delete([FromRoute]int id)
		{
			//todo check this logic to ensure correct claim scope is enforced (ie user with permissions of game with id 2 would pass permissions for group with id 2)
			var actorClaim = _actorClaimCoreController.Get(id);
			var claim = _claimCoreController.Get(actorClaim.ClaimId);
			if ((await _authorizationService.AuthorizeAsync(User, actorClaim.EntityId, HttpContext.ScopeItems(claim.ClaimScope))).Succeeded)
			{
				var claimCount = _actorClaimCoreController.GetClaimActors(actorClaim.ClaimId, actorClaim.EntityId).Count;
				if (claimCount > 1)
				{
					_actorClaimCoreController.Delete(id);
					return Ok();
				}
			}
			return Forbid();
		}
	}
}
