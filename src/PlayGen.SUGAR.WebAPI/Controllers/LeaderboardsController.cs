using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;

using Microsoft.AspNetCore.Mvc;

using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.WebAPI.Exceptions;
using PlayGen.SUGAR.GameData;
using PlayGen.SUGAR.WebAPI.Controllers.Filters;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
	/// <summary>
	/// Web Controller that facilitates Leaderboard specific operations.
	/// </summary>
	[Route("api/[controller]")]
	public class LeaderboardsController : Controller
	{
		private readonly Data.EntityFramework.Controllers.LeaderboardController _leaderboardController;
		private readonly LeaderboardController _leaderboardEvaluationController;

		public LeaderboardsController(Data.EntityFramework.Controllers.LeaderboardController leaderboardController,
			LeaderboardController leaderboardEvaluationController)
		{
			_leaderboardController = leaderboardController;
			_leaderboardEvaluationController = leaderboardEvaluationController;
		}

	}
}