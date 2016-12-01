using System.Collections.Generic;
using PlayGen.SUGAR.Client.AsyncRequestQueue;
using PlayGen.SUGAR.Client.EvaluationEvents;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client
{
	/// <summary>
	/// Controller that facilitates Game specific operations.
	/// </summary>
	public class GameClient : ClientBase
	{
		private const string ControllerPrefix = "api/game";

		public GameClient(string baseAddress, IHttpHandler httpHandler, RequestController asyncRequestController, EvaluationNotifications evaluationNotifications)
			: base(baseAddress, httpHandler, asyncRequestController, evaluationNotifications)
		{
		}

		/// <summary>
		/// Get a list of all Games.
		/// </summary>
		/// <returns>A list of <see cref="GameResponse"/> that hold Games details.</returns>
		public IEnumerable<GameResponse> Get()
		{
			var query = GetUriBuilder(ControllerPrefix + "/list").ToString();
			return Get<IEnumerable<GameResponse>>(query);
		}

		/// <summary>
		/// Get a list of Games that match <param name="name"/> provided.
		/// </summary>
		/// <param name="name">Game name</param>
		/// <returns>A list of <see cref="GameResponse"/> which match the search criteria.</returns>
		public IEnumerable<GameResponse> Get(string name)
		{
			var query = GetUriBuilder(ControllerPrefix + "/find/{0}", name).ToString();
			
			return Get<IEnumerable<GameResponse>>(query);
		}

		/// <summary>
		/// Get Game that matches <param name="id"/> provided.
		/// </summary>
		/// <param name="id">Game id</param>
		/// <returns><see cref="GameResponse"/> which matches search criteria.</returns>
		public GameResponse Get(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/findbyid/{0}", id).ToString();
			return Get<GameResponse>(query, new[] { System.Net.HttpStatusCode.OK, System.Net.HttpStatusCode.NoContent });
		}

		/// <summary>
		/// Create a new Game.
		/// Requires the <see cref="GameRequest.Name"/> to be unique.
		/// </summary>
		/// <param name="game"><see cref="GameRequest"/> object that contains the details of the new Game.</param>
		/// <returns>A <see cref="GameResponse"/> containing the new Game details.</returns>
		public GameResponse Create(GameRequest game)
		{
			var query = GetUriBuilder(ControllerPrefix + "").ToString();
			return Post<GameRequest, GameResponse>(query, game);
		}

		/// <summary>
		/// Update an existing Game.
		/// </summary>
		/// <param name="id">Id of the existing Game.</param>
		/// <param name="game"><see cref="GameRequest"/> object that holds the details of the Game.</param>
		public void Update(int id, GameRequest game)
		{
			var query = GetUriBuilder(ControllerPrefix + "/update/{0}", id).ToString();
			Put(query, game);
		}

		/// <summary>
		/// Delete Game with the ID provided.
		/// </summary>
		/// <param name="id">Game ID.</param>
		public void Delete(int id)
		{
			var query = GetUriBuilder(ControllerPrefix + "/{0}", id).ToString();
			Delete(query);
		}
	}
}
