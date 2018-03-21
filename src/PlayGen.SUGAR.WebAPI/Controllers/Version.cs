using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	[Route("api/[controller]")]
	public class VersionController : Controller
	{
		// GET
		[HttpGet]
		public IActionResult Get()
		{
			return new OkObjectResult($"{DateTime.UtcNow}: {typeof(VersionController).GetTypeInfo().Assembly}. API Version: 0.0.0");
		}
	}
}
