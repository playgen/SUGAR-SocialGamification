﻿namespace PlayGen.SUGAR.Common
{
    public class EvaluationProgress
    {
        // todo should probably store token rather than name

        public Actor Actor { get; set; }

        public string Name { get; set; }

        public float Progress { get; set; }
    }
}