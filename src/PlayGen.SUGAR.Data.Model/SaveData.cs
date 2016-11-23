using System;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
    public abstract class SaveData : IModificationHistory
	{
		public int Id { get; set; }

		public int? ActorId { get; set; }

		public int? GameId { get; set; }

		public GameDataCategory Category { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public Common.Shared.SaveDataType SaveDataType { get; set; }

		public abstract Common.Shared.DataCategory DataCategory { get; }

		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}
