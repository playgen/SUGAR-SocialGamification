using System;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Interfaces
{
	public interface IGameDataController
	{
		float SumFloats(int? gameId, int? actorId, string key);

		long SumLongs(int? gameId, int? actorId, string key);

		float GetHighestFloats(int? gameId, int? actorId, string key);

		long GetHighestLongs(int? gameId, int? actorId, string key);

		float GetLowestFloats(int? gameId, int? actorId, string key);

		long GetLowestLongs(int? gameId, int? actorId, string key);

		bool KeyExists(int? gameId, int? actorId, string key);

		bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latestBool);

		bool TryGetLatestString(int? gameId, int? actorId, string key, out string latestString);

		int CountKeys(int? gameId, int? actorId, string key, GameDataType gameDataType);

		DateTime TryGetLatestKey(int? gameId, int? actorId, string key, GameDataType gameDataType);

		DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, GameDataType gameDataType);

		void Create(GameData data);
	}
}