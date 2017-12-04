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
	/// Web Controller that facilitates ActorData specific operations.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize("Bearer")]
	[ValidateSession]
	public class ActorDataController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.ActorDataController _actorDataCoreController;

		public ActorDataController(Core.Controllers.ActorDataController actorDataCoreController, IAuthorizationService authorizationService)
		{
			_actorDataCoreController = actorDataCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Find a list of all ActorData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/actordata?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="EvaluationDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		[Authorization(ClaimScope.User, AuthorizationAction.Get, AuthorizationEntity.ActorData)]
		[Authorization(ClaimScope.Group, AuthorizationAction.Get, AuthorizationEntity.ActorData)]
		public async Task<IActionResult> Get(int? actorId, int? gameId, string[] key)
		{
			if ((await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, actorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var data = _actorDataCoreController.Get(gameId, actorId, key);
				var dataContract = data.ToContractList();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new ActorData record.
		/// 
		/// Example Usage: POST api/actordata
		/// </summary>
		/// <param name="newData"><see cref="EvaluationDataRequest"/> object that holds the details of the new ActorData.</param>
		/// <returns>A <see cref="EvaluationDataResponse"/> containing the new ActorData details.</returns>
		[HttpPost]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationAction.Create, AuthorizationEntity.ActorData)]
		[Authorization(ClaimScope.User, AuthorizationAction.Create, AuthorizationEntity.ActorData)]
		public async Task<IActionResult> Add([FromBody]EvaluationDataRequest newData)
		{
			if ((await _authorizationService.AuthorizeAsync(User, newData.CreatingActorId, HttpContext.ScopeItems(ClaimScope.Group))).Succeeded ||
				(await _authorizationService.AuthorizeAsync(User, newData.CreatingActorId, HttpContext.ScopeItems(ClaimScope.User))).Succeeded)
			{
				var data = newData.ToActorDataModel();
				var exists = _actorDataCoreController.KeyExists(data.GameId, data.ActorId, data.Key);
				if (exists)
				{
					var existing = _actorDataCoreController.Get(data.GameId, data.ActorId, new[] { data.Key });
					var firstData = existing.ElementAt(0);
					data.Id = firstData.Id;
					_actorDataCoreController.Update(data);
				}
				else
				{
					_actorDataCoreController.Add(data);
				}
				var dataContract = data.ToContract();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}
	}
}