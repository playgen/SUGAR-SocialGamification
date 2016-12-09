using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayGen.SUGAR.Authorization;
using PlayGen.SUGAR.Common.Shared.Permissions;
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

        // Method for system admins to create a match
        [HttpGet("create/{gameId:int}")]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Create, AuthorizationOperation.Match)]
        public IActionResult Create(int gameId)
        {
            var userId = HttpContext.Request.Headers.GetUserId();

            if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var match = _matchCoreController.Create(gameId, userId);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        [HttpGet("create")]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        public IActionResult Create()
        {
            var userId = HttpContext.Request.Headers.GetUserId();
            var gameId = HttpContext.Request.Headers.GetUserId();

            if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
                var match = _matchCoreController.Create(gameId, userId);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        [HttpGet("createandstart")]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Create, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Create, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Create, AuthorizationOperation.Match)]
        public IActionResult CreateAndStart()
        {
            var gameId = HttpContext.Request.Headers.GetGameId();
            var userId = HttpContext.Request.Headers.GetUserId();

            if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
                var match = _matchCoreController.Create(gameId, userId);
                match = _matchCoreController.Start(match.Id);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        [HttpGet("{matchId:int}/start")]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        public IActionResult Start(int matchId)
        {
            var gameId = HttpContext.Request.Headers.GetGameId();
            var userId = HttpContext.Request.Headers.GetUserId();

            if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
                var match = _matchCoreController.Start(matchId);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        [HttpGet("{matchId:int}/end")]
        [Authorization(ClaimScope.Group, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.User, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        [Authorization(ClaimScope.Game, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        public IActionResult End([FromRoute]int matchId)
        {
            var gameId = HttpContext.Request.Headers.GetGameId();
            var userId = HttpContext.Request.Headers.GetUserId();

            if (_authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GroupRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["UserRequirements"]).Result ||
                _authorizationService.AuthorizeAsync(User, userId, (AuthorizationRequirement)HttpContext.Items["GameRequirements"]).Result)
            {
                var match = _matchCoreController.End(matchId);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        // Method for system admins to end a match
        [HttpGet("{gameId:int}/{matchId:int}/end")]
        [ArgumentsNotNull]
        [Authorization(ClaimScope.Global, AuthorizationOperation.Update, AuthorizationOperation.Match)]
        public IActionResult End([FromRoute]int gameId, [FromRoute]int matchId)
        {
            var userId = HttpContext.Request.Headers.GetUserId();

            if (_authorizationService.AuthorizeAsync(User, gameId, (AuthorizationRequirement)HttpContext.Items["Requirements"]).Result)
            {
                var match = _matchCoreController.End(matchId);
                var contract = match.ToContract();
                return new ObjectResult(contract);
            }
            return Forbid();
        }

        [HttpGet("{start:datetime}/{end:datetime}")]
        public IActionResult GetByTime(DateTime? start, DateTime? end)
        {
            var matches = _matchCoreController.GetByTime(start, end);
            return new ObjectResult(matches.ToContractList());
        }

        [HttpGet("game/{gameId:int}")]
        [ArgumentsNotNull]
        public IActionResult GetByGame(int gameId)
        {
            var matches = _matchCoreController.GetByGame(gameId);
            return new ObjectResult(matches.ToContractList());
        }

        [HttpGet("game/{gameId:int}/{start:datetime}/{end:datetime}")]
        public IActionResult GetByGame(int gameId, DateTime? start, DateTime? end)
        {
            var matches = _matchCoreController.GetByGame(gameId, start, end);
            return new ObjectResult(matches.ToContractList());
        }

        [HttpGet("creator/{creatorId:int}")]
        [ArgumentsNotNull]
        public IActionResult GetByCreator(int creatorId)
        {
            var matches = _matchCoreController.GetByCreator(creatorId);
            return new ObjectResult(matches.ToContractList());
        }

        [HttpGet("creator/{creatorId:int}/{start:datetime}/{end:datetime}")]
        public IActionResult GetByCreator(int creatorId, DateTime? start, DateTime? end)
        {
            var matches = _matchCoreController.GetByCreator(creatorId, start, end);
            return new ObjectResult(matches.ToContractList());
        }

        [HttpGet("game/{gameId:int}/creator/{creatorId:int}")]
        [ArgumentsNotNull]
        public IActionResult GetByGameAndCreator(int gameId, int creatorId)
        {
            var matches = _matchCoreController.GetByGameAndCreator(gameId, creatorId);
            return new ObjectResult(matches.ToContractList());
        }

        [HttpGet("game/{gameId:int}/creator/{creatorId:int}/{start:datetime}/{end:datetime}")]
        public IActionResult GetByGameAndCreator(int gameId, int creatorId, DateTime? start, DateTime? end)
        {
            var matches = _matchCoreController.GetByGameAndCreator(gameId, creatorId, start, end);
            return new ObjectResult(matches.ToContractList());
        }
    }
}
