using System;

namespace PlayGen.SUGAR.Common.Shared.Utilities
{
    public interface ILogger
    {
        void Debug(string message);

        void Warning(string message);
        
        void Exception(Exception exception);
    }
}
