using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.EvaluationEvents
{
	public class EvaluationNotification
	{
		/// <summary>
		///     The details of the actor whose progress was being checked.
		/// </summary>
		public ActorResponse Actor { get; set; }

		/// <summary>
		///     The name of the achievement/skill which progress was being checked for.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     Progress of current achievement/skill [0 to 1].
		/// </summary>
		public float Progress { get; set; }

		/// <summary>
		///     Skill or Achievement
		/// </summary>
		public EvaluationType Type { get; set; }
	}
}