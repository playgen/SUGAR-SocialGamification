using PlayGen.SUGAR.Common.Exceptions;

namespace PlayGen.SUGAR.Server.WebAPI.Exceptions
{
    public class ClaimNotFoundException : SUGARException
    {
        protected ClaimNotFoundException(string message) : base(message)
        {
        }
    }
}
