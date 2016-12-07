using PlayGen.SUGAR.Common.Shared.Exceptions;

namespace PlayGen.SUGAR.ServerAuthentication.Exceptions
{
    public class ClaimNotFoundException : SUGARException
    {
        public ClaimNotFoundException(string message) : base(message)
        {
        }
    }
}
