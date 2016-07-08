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
		/// Find a list of all Resources that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="ResourceResponse"/> which match the search criteria.</returns>
		public IEnumerable<ResourceResponse> Get(int actorId, int gameId, string[] key)
		{
			var query = GetUriBuilder(ControllerPrefix)
				.AppendQueryParameter(actorId, "actorId={0}")
				.AppendQueryParameter(gameId, "gameId={0}")
				.AppendQueryParameters(key, "key={0}")
				.ToString();
			return Get<IEnumerable<ResourceResponse>>(query);
		}

		/// <summary>
		/// Create a new Resource record.
		/// </summary>
		/// <param name="data"><see cref="ResourceRequest"/> object that holds the details of the new Resource.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the new Resource details.</returns>
		public ResourceResponse Add(ResourceRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Post<ResourceRequest, ResourceResponse>(query, data);
		}

		/// <summary>
		/// Create a an updated Resource record.
		/// </summary>
		/// <param name="data"><see cref="ResourceRequest"/> object that holds the details of the updated Resource.</param>
		/// <returns>A <see cref="ResourceResponse"/> containing the updated Resource details.</returns>
		public ResourceResponse Update(ResourceRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix + "/update").ToString();
			return Post<ResourceRequest, ResourceResponse>(query, data);
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
