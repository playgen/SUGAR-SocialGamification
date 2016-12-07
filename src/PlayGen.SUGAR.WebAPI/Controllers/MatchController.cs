using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.ServerAuthentication.Extensions;
using PlayGen.SUGAR.WebAPI.Attributes;
using PlayGen.SUGAR.WebAPI.Extensions;

namespace PlayGen.SUGAR.WebAPI.Controllers
{
    // todo add comments

    [Route("api/[controller]")]
    [Authorize("Bearer")]
    [ValidateSession]
    public class MatchController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Core.Controllers.MatchController _matchCoreController;

        public MatchController(Core.Controllers.MatchController matchCoreController,
            IAuthorizationService authorizationService)
        {
            _matchCoreController = matchCoreController;
            _authorizationService = authorizationService;
        }

        [HttpGet("start")]
        [ArgumentsNotNull]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Match)]
        public IActionResult Start()
        {
            var gameId = HttpContext.Request.GetGameId();
            var userId = HttpContext.Request.GetUserId();
            
            if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
                var match = _matchCoreController.Start(gameId, userId);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        [HttpGet("{matchId:int}/end")]
        [ArgumentsNotNull]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        public IActionResult End([FromRoute]int matchId)
        {
            var gameId = HttpContext.Request.GetGameId();
            var userId = HttpContext.Request.GetUserId();

            if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
                var match = _matchCoreController.Start(gameId, userId);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        public IActionResult GetByTime(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public IActionResult GetByGame(int gameId, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            throw new NotImplementedException();
        }
        
        public IActionResult GetByCreator(int creatorId, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            throw new NotImplementedException();
        }

        public IActionResult GetByGameAndCreator(int gameId, int creatorId, DateTime start = default(DateTime), DateTime end = default(DateTime))
        {
            throw new NotImplementedException();
        }
    }
}
