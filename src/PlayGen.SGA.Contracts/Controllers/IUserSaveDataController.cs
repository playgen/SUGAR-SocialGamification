using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IUserSaveDataController
    {
        IEnumerable<SaveDataResponse> Get(int actorId, int gameId, string[] keys);

        SaveDataResponse Add(SaveDataRequest data);
    }
}
