using System;

namespace PlayGen.SUGAR.Common.Shared.Extensions
{
    public static class EvaluationExtensions
    {
        public static SaveDataCategory ToSaveDataCategory(this EvaluationType evaluationType)
        {
            switch (evaluationType)
            {
                case EvaluationType.Achievement:
                    return SaveDataCategory.Achievement;

                case EvaluationType.Skill:
                    return SaveDataCategory.Skill;

                default:
                    throw new ArgumentOutOfRangeException(nameof(evaluationType), evaluationType, null);
            }
        }
    }
}
