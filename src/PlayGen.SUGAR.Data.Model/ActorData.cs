using System;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class ActorData : IModificationHistory
	{
		public int Id { get; set; }

		public int? ActorId { get; set; }

		public int? GameId { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public Common.Shared.EvaluationDataType EvaluationDataType { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}