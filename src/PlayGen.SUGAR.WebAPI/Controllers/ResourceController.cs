using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
		private readonly Data.EntityFramework.Controllers.ResourceController _resourceController;

		public ResourceController(Data.EntityFramework.Controllers.ResourceController resourceController)
		{
			_resourceController = resourceController;
		}

		/// <summary>
		/// Find a list of all Resources that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// 
		/// Example Usage: GET api/resource?actorId=1&amp;gameId=1&amp;key=key1&amp;key=key2
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="ResourceResponse"/> which match the search criteria.</returns>
		[HttpGet]
		[ResponseType(typeof(IEnumerable<ResourceResponse>))]
		public IActionResult Get(int? actorId, int? gameId, string[] key)
		{
			var data = _resourceController.Get(actorId, gameId, key);
			var dataContract = data.ToResourceContractList();
			return new ObjectResult(dataContract);
		}

		/// <summary>
		/// Create a new GameData record.
		/// 
		/// Example Usage: POST api/gamedata
		/// </summary>
		/// <param name="newData"><see cref="ResourceRequest"/> object that holds the details of the new ResourceData.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the new GameData details.</returns>
		[HttpPost]
		[ResponseType(typeof(ResourceResponse))]
		[ArgumentsNotNull]
		public IActionResult Add([FromBody]ResourceRequest newData)
		{
			var data = newData.ToModel();
			_resourceController.Create(data);
			var dataContract = data.ToResourceContract();
			return new ObjectResult(dataContract);
		}
	}
}
