using PlayGen.SUGAR.Common.Exceptions;

namespace PlayGen.SUGAR.WebAPI.Exceptions
{
    public class ClaimNotFoundException : SUGARException
    {
        protected ClaimNotFoundException(string message) : base(message)
        {
        }
    }
}
