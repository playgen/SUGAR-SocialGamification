using System;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public abstract class GameData : IGameData, IRecord, IModificationHistory
	{
		public int Id { get; set; }
		
		public abstract int ActorId { get; } 

		public int GameId { get; set; }

		public virtual Game Game { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public GameDataValueType DataType { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}