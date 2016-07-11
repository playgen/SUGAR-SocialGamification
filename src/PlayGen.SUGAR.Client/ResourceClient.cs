using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Resource specific operations.
	/// </summary>
	public class ResourceClient : ClientBase
	{
		private const string ControllerPrefix = "api/resource";

		public ResourceClient(string baseAddress, Credentials credentials) : base(baseAddress, credentials)
		{
		}

		/// <summary>
		/// Find a list of all Resources that match the <param name="gameId"/>, <param name="actorId"/> and <param name="keys"/> provided.
		/// </summary>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="ResourceResponse"/> which match the search criteria.</returns>
		public IEnumerable<ResourceResponse> Get(int? gameId, int? actorId, string[] keys)
		{
			var query = GetUriBuilder(ControllerPrefix)
				.AppendQueryParameter(gameId, "gameId={0}")
				.AppendQueryParameter(actorId, "actorId={0}")
				.AppendQueryParameters(keys, "keys={0}")
				.ToString();
			return Get<IEnumerable<ResourceResponse>>(query);
		}

		/// <summary>
		/// Create a new Resource record.
		/// </summary>
		/// <param name="data"><see cref="ResourceAddRequest"/> object that holds the details of the new Resource.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the new Resource details.</returns>
		public ResourceResponse Add(ResourceAddRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Post<ResourceAddRequest, ResourceResponse>(query, data);
		}

		/// <summary>
		/// Create a an updated Resource record.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="data"><see cref="ResourceUpdateRequest"/> object that holds the details of the updated Resource.</param>
		public void Update(int id, ResourceUpdateRequest data)
		{
			var query = GetUriBuilder($"{ControllerPrefix}/update/{id}").ToString();
			Put(query, data);
		}

		/// <summary>
		/// Transfers a quantity of a specific resource.
		/// </summary>
		/// <param name="data"></param>
		/// <returns>A <see cref="ResourceTransferResponse"/> containing the modified resources.</returns>
		public ResourceTransferResponse Transfer(ResourceTransferRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix + "/transfer").ToString();
			return Post<ResourceTransferRequest, ResourceTransferResponse>(query, data);
		}
	}
}
