using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IGroupSaveDataController
    {
        SaveDataResponse Add(SaveDataRequest data);

        IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] keys);
    }
}
