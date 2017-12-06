using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	[Route("api/[controller]")]
	public class VersionController : Controller
	{
		// GET
		[HttpGet]
		public IActionResult Get()
		{
			return new OkObjectResult($"{DateTime.UtcNow}: {typeof(VersionController).GetTypeInfo().Assembly}." +
									$" \nAPI Version: {APIVersion.Version}");
		}
	}
}
