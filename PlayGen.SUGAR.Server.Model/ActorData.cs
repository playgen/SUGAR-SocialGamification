using System;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Model.Interfaces;

namespace PlayGen.SUGAR.Server.Model
{
	public class ActorData : IModificationHistory
	{
		public int Id { get; set; }

		public int? ActorId { get; set; }

		public int? GameId { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public EvaluationDataType EvaluationDataType { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}