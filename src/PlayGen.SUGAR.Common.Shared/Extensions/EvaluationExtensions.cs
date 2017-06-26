using System;

namespace PlayGen.SUGAR.Common.Extensions
{
	public static class EvaluationExtensions
	{
		public static EvaluationDataCategory ToEvaluationDataCategory(this EvaluationType evaluationType)
		{
			switch (evaluationType)
			{
				case EvaluationType.Achievement:
					return EvaluationDataCategory.Achievement;

				case EvaluationType.Skill:
					return EvaluationDataCategory.Skill;

				default:
					throw new ArgumentOutOfRangeException(nameof(evaluationType), evaluationType, null);
			}
		}
	}
}