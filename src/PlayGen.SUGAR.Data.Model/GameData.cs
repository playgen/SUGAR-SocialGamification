using System;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class GameData : IModificationHistory
	{
		public long Id { get; set; }
		
		public int? ActorId { get; set; } 

		public int? GameId { get; set; }

		//public virtual Game Game { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public GameDataType DataType { get; set; }


		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}