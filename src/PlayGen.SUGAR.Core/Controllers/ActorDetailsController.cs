using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.Core.Controllers
{
	public class ActorDetailsController
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly Data.EntityFramework.Controllers.ActorDetailsController _actorDetailsDbController;

		public ActorDetailsController(Data.EntityFramework.Controllers.ActorDetailsController actorDetailsDbController)
		{
			_actorDetailsDbController = actorDetailsDbController;
		}

		public void AddOrUpdate(ActorDetails actorDetails)
		{
			var exists = KeyExists(actorDetails.ActorId, actorDetails.Key);
			if (exists)
			{
				var existing = Get(actorDetails.ActorId, new[] {actorDetails.Key}).First();
				actorDetails.Id = existing.Id;
				Update(actorDetails);
			}
			else
			{
				Add(actorDetails);
			}
		}

		public List<ActorDetails> Get(int actorId, ICollection<string> keys = null)
		{
			var datas = _actorDetailsDbController.Get(actorId, keys);

			Logger.Debug($"{datas?.Count} Actor Data for ActorId: {actorId}, Keys: {string.Join(", ", keys)}");

			return datas;
		}

		public bool KeyExists(int actorId, string key)
		{
			var keyExists = _actorDetailsDbController.KeyExists(actorId, key);

			Logger.Info($"Key Exists: {keyExists} ActorId: {actorId}, Key: {key}");

			return keyExists;
		}

		public ActorDetails Add(ActorDetails newDetails)
		{
			if (ParseCheck(newDetails))
			{
				newDetails = _actorDetailsDbController.Create(newDetails);

				Logger.Info($"{newDetails?.Id}");

				return newDetails;
			}
			else
			{
				throw new ArgumentException($"Invalid Value {newDetails.Value} for EvaluationDataType {newDetails.EvaluationDataType}");
			}
		}

		public void Update(ActorDetails newDetails)
		{
			_actorDetailsDbController.Update(newDetails);

			Logger.Info($"{newDetails?.Id}");
		}

		protected bool ParseCheck(ActorDetails details)
		{
			switch (details.EvaluationDataType)
			{
				case EvaluationDataType.String:
					return true;

				case EvaluationDataType.Long:
					long tryLong;
					return long.TryParse(details.Value, out tryLong);

				case EvaluationDataType.Float:
					float tryFloat;
					return float.TryParse(details.Value, out tryFloat);

				case EvaluationDataType.Boolean:
					bool tryBoolean;
					return bool.TryParse(details.Value, out tryBoolean);

				default:
					return false;
			}
		}
	}
}
