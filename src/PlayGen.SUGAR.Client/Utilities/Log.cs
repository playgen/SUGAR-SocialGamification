using System;
using PlayGen.SUGAR.Common.Shared.Utilities;

namespace PlayGen.SUGAR.Client.Utilities
{
    public static class Log
    {
        private static ILogger _logger;

        public static void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static void Debug(string message)
        {
            _logger?.Debug(message);
        }

        public static void Warning(string message)
        {
            _logger?.Warning(message);
        }

        public static void Exception(Exception exception)
        {
            _logger?.Exception(exception);
        }
    }
}
