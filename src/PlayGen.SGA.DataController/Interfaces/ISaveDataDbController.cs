namespace PlayGen.SGA.DataController.Interfaces
{
    public interface ISaveDataDbController
    {
        float SumFloats(int gameId, int actorId, string key);

        long SumLongs(int gameId, int actorId, string key);

        bool TryGetLatestBool(int gameId, int actorId, string key, out bool latestBool);

        bool TryGetLatestString(int gameId, int actorId, string key, out string latestString);
    }
}