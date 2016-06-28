using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates GroupData specific operations.
	/// </summary>
	public class GroupDataClient : ClientBase, IGroupSaveDataController
	{
		public GroupDataClient(string baseAddress) : base(baseAddress)
		{
		}

		/// <summary>
		/// Get a list of all GroupData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="keys"/> provided.
		/// </summary>
		/// <param name="actorId">ID of a Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="keys">Array of Key names.</param>
		/// <returns>A list of <see cref="GameDataResponse"/> which match the search criteria.</returns>
		public IEnumerable<GameDataResponse> Get(int actorId, int gameId, string[] keys)
		{
			var query = GetUriBuilder("api/groupdata")
				.AppendQueryParameter(actorId, "actorId={0}")
				.AppendQueryParameter(gameId, "gameId={0}")
				.AppendQueryParameters(keys, "key={0}")
				.ToString();
			return Get<IEnumerable<GameDataResponse>>(query);
		}

		/// <summary>
		/// Create a new GroupData record.
		/// </summary>
		/// <param name="data"><see cref="SaveDataRequest"/> object that holds the details of the new GroupData.</param>
		/// <returns>A <see cref="GameDataResponse"/> containing the new GroupData details.</returns>
		public GameDataResponse Add(SaveDataRequest data)
		{
			var query = GetUriBuilder("api/groupdata").ToString();
			return Post<SaveDataRequest, GameDataResponse>(query, data);
		}
	}
}
