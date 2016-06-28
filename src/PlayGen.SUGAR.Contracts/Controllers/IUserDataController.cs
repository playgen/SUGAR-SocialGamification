using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts.Controllers
{
	public interface IUserDataController
	{
		IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] keys);

		SaveDataResponse Add(SaveDataRequest data);
	}
}
