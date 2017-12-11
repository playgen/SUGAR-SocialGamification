using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.WebAPI.Attributes;

namespace PlayGen.SUGAR.Server.WebAPI.Controllers
{
	[Route("api/[controller]")]
	public class VersionController : Controller
	{
		// GET
		[HttpGet]
		[AllowAnyAPIVersion]
		[AllowWithoutSession]
		public IActionResult Get()
		{
			return new OkObjectResult($"{DateTime.UtcNow}: {typeof(VersionController).GetTypeInfo().Assembly}." +
									$" {APIVersion.Key}: {APIVersion.Version}");
		}
	}
}
