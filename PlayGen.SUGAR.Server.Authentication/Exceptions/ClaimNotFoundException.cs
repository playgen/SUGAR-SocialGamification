using PlayGen.SUGAR.Common.Exceptions;

namespace PlayGen.SUGAR.Server.Authentication.Exceptions
{
    public class ClaimNotFoundException : SUGARException
    {
        public ClaimNotFoundException(string message) : base(message)
        {
        }
    }
}
