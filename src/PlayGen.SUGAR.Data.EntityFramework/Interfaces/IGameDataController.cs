using System;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Interfaces
{
	public interface IGameDataController
	{
		float SumFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime));

		long SumLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime));

		float GetHighestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime));

		long GetHighestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime));

		float GetLowestFloats(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime));

		long GetLowestLongs(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime));

		bool KeyExists(int? gameId, int? actorId, string key, DateTime start = default(DateTime), DateTime end = default(DateTime));

		bool TryGetLatestBool(int? gameId, int? actorId, string key, out bool latestBool, DateTime start = default(DateTime), DateTime end = default(DateTime));

		bool TryGetLatestString(int? gameId, int? actorId, string key, out string latestString, DateTime start = default(DateTime), DateTime end = default(DateTime));

		int CountKeys(int? gameId, int? actorId, string key, GameDataType gameDataType, DateTime start = default(DateTime), DateTime end = default(DateTime));

		DateTime TryGetLatestKey(int? gameId, int? actorId, string key, GameDataType gameDataType, DateTime start = default(DateTime), DateTime end = default(DateTime));

		DateTime TryGetEarliestKey(int? gameId, int? actorId, string key, GameDataType gameDataType, DateTime start = default(DateTime), DateTime end = default(DateTime));

		void Create(GameData data);
	}
}