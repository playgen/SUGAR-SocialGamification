using System.Collections.Generic;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Contracts.Controllers;

namespace PlayGen.SUGAR.Client
{
	public class GameDataClient : ClientBase
	{
		private const string ControllerPrefix = "api/gamedata";

		public GameDataClient(string baseAddress) : base(baseAddress)
		{
		}



		/// <summary>
		/// Get a list of all UserData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// </summary>
		/// <param name="actorId">ID of a User.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="GameDataResponse"/> which match the search criteria.</returns>
		public IEnumerable<GameDataResponse> Get(int actorId, int gameId, string[] key)
		{
			var query = GetUriBuilder(ControllerPrefix)
				.AppendQueryParameter(actorId, "actorId={0}")
				.AppendQueryParameter(gameId, "gameId={0}")
				.AppendQueryParameters(key, "key={0}")
				.ToString();
			return Get<IEnumerable<GameDataResponse>>(query);
		}

		/// <summary>
		/// Create a new UserData record.
		/// </summary>
		/// <param name="data"><see cref="GameDataRequest"/> object that holds the details of the new UserData.</param>
		/// <returns>A <see cref="GameDataResponse"/> containing the new UserData details.</returns>
		public GameDataResponse Add(GameDataRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Post<GameDataRequest, GameDataResponse>(query, data);
		}
	}
}
