using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.WebAPI.Extensions;
using System.Linq;
using PlayGen.SUGAR.WebAPI.Attributes;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates ActorDetails specific operations.
	/// </summary>
	[Authorize("Bearer")]
	[ValidateSession]
	public class ActorDetailsController : Controller
	{
		private readonly IAuthorizationService _authorizationService;
		private readonly Core.Controllers.ActorDetailsController _actorDetailsCoreController;

		public ActorDetailsController(Core.Controllers.ActorDetailsController actorDetailsCoreController,
					IAuthorizationService authorizationService)
		{
			_actorDetailsCoreController = actorDetailsCoreController;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Find a list of all ActorDetails that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/actordata?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="EvaluationDataResponse"/> which match the search criteria.</returns>
		[HttpGet]
		[Route("api/actor/{actorId:int}/details")]
		//[ResponseType(typeof(IEnumerable<EvaluationDataResponse>))]
		[Authorization(ClaimScope.User, AuthorizationOperation.Get, AuthorizationOperation.ActorData)]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Get, AuthorizationOperation.ActorData)]
		public IActionResult Get([FromRoute]int actorId, [FromQuery] string[] key)
		{
			if (_authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result)
			{
				var data = _actorDetailsCoreController.Get(actorId, key);
				var dataContract = data.ToContractList();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}

		/// <summary>
		/// Create a new ActorDetails record.
		/// 
		/// Example Usage: POST api/actordata
		/// </summary>
		/// <param name="actorId"></param>
		/// <param name="newData"><see cref="EvaluationDataRequest"/> object that holds the details of the new ActorDetails.</param>
		/// <returns>A <see cref="EvaluationDataResponse"/> containing the new ActorDetails details.</returns>
		[HttpPost]
		[Route("api/actor/{actorId:int}/details")]
		//[ResponseType(typeof(EvaluationDataResponse))]
		[ArgumentsNotNull]
		[Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.ActorData)]
		[Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.ActorData)]
		public IActionResult Add([FromRoute]int actorId, [FromBody]ActorDetailsRequest newData)
		{
			if (_authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
				_authorizationService.AuthorizeAsync(User, actorId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result)
			{
				var data = newData.ToActorDataModel();
				_actorDetailsCoreController.AddOrUpdate(data);
				var dataContract = data.ToContract();
				return new ObjectResult(dataContract);
			}
			return Forbid();
		}
	}
}