using System.Collections.Generic;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Client.Extensions;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates GameData specific operations.
	/// </summary>
	public class GameDataClient : ClientBase
	{
		private const string ControllerPrefix = "api/gamedata";

		public GameDataClient(string baseAddress, IHttpHandler httpHandler, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, evaluationNotifications)
		{
		}

		/// <summary>
		/// Find a list of all GameData that match the <param name="actorId"/>, <param name="gameId"/> and <param name="key"/> provided.
		/// </summary>
		/// <param name="actorId">ID of a User/Group.</param>
		/// <param name="gameId">ID of a Game.</param>
		/// <param name="key">Array of Key names.</param>
		/// <returns>A list of <see cref="GameDataResponse"/> which match the search criteria.</returns>
		public IEnumerable<GameDataResponse> Get(int? actorId, int? gameId, string[] key)
		{
			var query = GetUriBuilder(ControllerPrefix)
				.AppendQueryParameter(actorId, "actorId={0}")
				.AppendQueryParameter(gameId, "gameId={0}")
				.AppendQueryParameters(key, "key={0}")
				.ToString();
			return Get<IEnumerable<GameDataResponse>>(query);
		}

		/// <summary>
		/// Create a new GameData record.
		/// </summary>
		/// <param name="data"><see cref="GameDataRequest"/> object that holds the details of the new GameData.</param>
		/// <returns>A <see cref="GameDataResponse"/> containing the new GameData details.</returns>
		public GameDataResponse Add(GameDataRequest data)
		{
			var query = GetUriBuilder(ControllerPrefix).ToString();
			return Post<GameDataRequest, GameDataResponse>(query, data);
		}
	}
}
