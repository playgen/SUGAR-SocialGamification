using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ActorDataController
	{
		private readonly Data.EntityFramework.Controllers.ActorDataController _actorDataDbController;

		public ActorDataController(Data.EntityFramework.Controllers.ActorDataController actorDataDbController)
		{
			_actorDataDbController = actorDataDbController;
		}

		public IEnumerable<ActorData> Get(int? gameId = null, int? actorId = null, IEnumerable<string> keys = null)
		{
			var datas = _actorDataDbController.Get(gameId, actorId, keys);
			return datas;
		}

		public bool KeyExists(int? gameId, int? actorId, string key)
		{
			var keyExists = _actorDataDbController.KeyExists(gameId, actorId, key);
			return keyExists;
		}

		public ActorData Add(ActorData newData)
		{
			if (ParseCheck(newData))
			{
				newData = _actorDataDbController.Create(newData);
				return newData;
			}
			else
			{
				throw new ArgumentException($"Invalid Value {newData.Value} for SaveDataType {newData.SaveDataType}");
			}
		}

		public void Update(ActorData newData)
		{
			_actorDataDbController.Update(newData);
		}

		protected bool ParseCheck(ActorData data)
		{
			switch (data.SaveDataType)
			{
				case SaveDataType.String:
					return true;

				case SaveDataType.Long:
					long tryLong;
					return long.TryParse(data.Value, out tryLong);

				case SaveDataType.Float:
					float tryFloat;
					return float.TryParse(data.Value, out tryFloat);

				case SaveDataType.Boolean:
					bool tryBoolean;
					return bool.TryParse(data.Value, out tryBoolean);

				default:
					return false;
			}
		}
	}
}
