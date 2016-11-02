using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts.Controllers
{
	public interface IUserDataController
	{
		IEnumerable<GameDataResponse> Get(int actorId, int gameId, string[] keys);

		GameDataResponse Add(GameDataRequest data);
	}
}
