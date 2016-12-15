using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class ActorDetails : IModificationHistory
	{
		public int Id { get; set; }

		public int ActorId { get; set; }

        public Actor Actor { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public Common.Shared.SaveDataType SaveDataType { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}