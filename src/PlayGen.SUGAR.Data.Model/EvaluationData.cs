using System;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model.Interfaces;

namespace PlayGen.SUGAR.Data.Model
{
	public class EvaluationData : IModificationHistory
	{
		public int Id { get; set; }

		public int? GameId { get; set; }

		public int? ActorId { get; set; }

		public int? MatchId { get; set; }

		public virtual Match Match { get; set; }
		
		public EvaluationDataCategory Category { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public EvaluationDataType EvaluationDataType { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateModified { get; set; }
	}
}