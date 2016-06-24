using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts.Controllers
{
	public interface IGroupSaveDataController
	{
		IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] keys);

		SaveDataResponse Add(SaveDataRequest data);
	}
}
