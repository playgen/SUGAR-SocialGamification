using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Interfaces
{
	public interface IGameDataController
	{
		float SumFloats(int? gameId, int? actorId, string key);

		long SumLongs(int? gameId, int? actorId, string key);

		bool KeyExists(int? gameId, int? actorId, string key);

		bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latestBool);

		bool TryGetLatestString(int? gameId, int? actorId, string key, out string latestString);

		void Create(GameData data);
	}
}