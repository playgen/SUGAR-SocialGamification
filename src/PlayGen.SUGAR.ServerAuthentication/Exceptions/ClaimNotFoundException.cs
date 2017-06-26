using PlayGen.SUGAR.Common.Exceptions;

namespace PlayGen.SUGAR.ServerAuthentication.Exceptions
{
    public class ClaimNotFoundException : SUGARException
    {
        public ClaimNotFoundException(string message) : base(message)
        {
        }
    }
}
