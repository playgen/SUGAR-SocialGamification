using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Extensions;
using System.Linq;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
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

		public ActorDataController(Core.Controllers.ActorDataController actorDataCoreController,
					IAuthorizationService authorizationService)
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
		//[ResponseType(typeof(IEnumerable<EvaluationDataResponse>))]
		[Authorization(ClaimScope.User, AuthorizationOperation.Get, AuthorizationOperation.ActorData)]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.ActorData)]
		public IActionResult Get(int? actorId, int? gameId, string[] key)
		{
			if (_authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result)
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
		//[ResponseType(typeof(EvaluationDataResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.ActorData)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.ActorData)]
		public IActionResult Add([FromBody]EvaluationDataRequest newData)
		{
			if (_authorizationService.AuthorizeAsync(User, newData.CreatingActorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, newData.CreatingActorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result)
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