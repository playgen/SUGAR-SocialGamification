﻿using System.ComponentModel.DataAnnotations;

using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Contracts
{
	/// <summary>
	/// Encapsulates requirements for completing an achievement or skill.
	/// </summary>
	/// <example>
	/// JSON
	/// {
	/// "EvaluationDataKey" : "Criteria Key",
	/// "EvaluationDataCategory" : "GameData",
	/// "EvaluationDataType" : "Long",
	/// "CriteriaQueryType" : "Any",
	/// "ComparisonType" : "Equals",
	/// "Scope" : "Actor",
	/// "Value" : "5"
	/// }
	/// </example>
	public class EvaluationCriteriaCreateRequest
	{
		/// <summary>
		/// The key which will be queried against to check if criteria is met.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string EvaluationDataKey { get; set; }

		/// <summary>
		/// EvaluationDataCategory of the key which is being queried.
		/// </summary>
		[Required]
		public EvaluationDataCategory? EvaluationDataCategory { get; set; }

		/// <summary>
		/// EvaluationDataType of the key which is being queried.
		/// </summary>
		[Required]
		public EvaluationDataType? EvaluationDataType { get; set; }

		/// <summary>
		/// Which stored EvaluationData will be queried.
		/// </summary>
		[Required]
		public CriteriaQueryType? CriteriaQueryType { get; set; }

		/// <summary>
		/// How the target value and the actual value will be compared.
		/// </summary>
		[Required]
		public ComparisonType? ComparisonType { get; set; }

		/// <summary>
		/// Whether the criteria will be checked against the actor or related actors (i.e. group members, user friends).
		/// </summary>
		[Required]
		public CriteriaScope? Scope { get; set; }

		/// <summary>
		/// The value which will compared against in order to see if the criteria has been met.
		/// </summary>
		[Required]
		[StringLength(64)]
		public string Value { get; set; }
	}
}