using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Core.Sessions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    /// <summary>
    /// Web Controller that facilitates session specific operations.
    /// </summary>
    [Authorize("Bearer")]
    [Route("api/[controller]")]
	public class SessionController : Controller
	{
	    private readonly SessionTracker _sessionTracker;

	    public SessionController(SessionTracker sessionTracker)
	    {
	        _sessionTracker = sessionTracker;
	    }

    	[HttpPost("heartbeat")]
        public IActionResult Heartbeat()
	    {
            return new ObjectResult(null);
        }
    }
}
