using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.Exceptions;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class MatchController
	{
		private readonly ILogger _logger;
		private readonly EntityFramework.Controllers.MatchController _matchDbController;
		private readonly EvaluationDataController _evaluationDataController;

		public MatchController(
			ILogger<MatchController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			SUGARContextFactory contextFactory, 
			EntityFramework.Controllers.MatchController matchDbController)
		{
			_logger = logger;
			_matchDbController = matchDbController;
			_evaluationDataController = new EvaluationDataController(evaluationDataLogger, contextFactory, EvaluationDataCategory.MatchData);
		}

		public Match Create(int gameId, int creatorId)
		{
			var match = new Match {
				GameId = gameId,
				CreatorId = creatorId,
			};

			_matchDbController.Create(match);

			_logger.LogInformation($"Match: {match.Id} created for Game: {gameId}, CreatorId: {creatorId}");

			return match;
		}

		public Match Start(int matchId)
		{
			var match = _matchDbController.Get(matchId);
			match.Started = DateTime.UtcNow;
			match = _matchDbController.Update(match);

			_logger.LogInformation($"Match: {match.Id} started");

			return match;
		}

		public Match End(int matchId)
		{
			var match = _matchDbController.Get(matchId);

			if (match.Started == null)
			{
				throw new Exceptions.InvalidOperationException($"The match {matchId} hasn't had its Started time set. " +
															   $"This must be set before setting the Ended time.");
			}

			match.Ended = DateTime.UtcNow;
			match = _matchDbController.Update(match);

			_logger.LogInformation($"Match: {match.Id} ended");

			return match;
		}

		public Match Get(int matchId)
		{
			var match = _matchDbController.Get(matchId);

			_logger.LogInformation($"Found {match?.Id}");

			return match;
		}

		public List<Match> GetByTime(DateTime? start, DateTime? end)
		{
			var results = _matchDbController.GetByTime(start, end);

			_logger.LogInformation($"{results.Count} Matches for Start: {start}, End: {end}");

			return results;
		}

		public List<Match> GetByGame(int gameId)
		{
			var results = _matchDbController.GetByGame(gameId);

			_logger.LogInformation($"{results.Count} Matches for GameId: {gameId}");

			return results;
		}

		public List<Match> GetByGame(int gameId, DateTime? start, DateTime? end)
		{
			var results = _matchDbController.GetByGame(gameId, start, end);

			_logger.LogInformation($"{results.Count} Matches for GameId: {gameId}, Start: {start}, End: {end}");

			return results;
		}

		public List<Match> GetByCreator(int creatorId)
		{
			var results = _matchDbController.GetByCreator(creatorId);

			_logger.LogInformation($"{results.Count} Matches for Creator: {creatorId}");

			return results;
		}

		public List<Match> GetByCreator(int creatorId, DateTime? start, DateTime? end)
		{
			var results = _matchDbController.GetByCreator(creatorId, start, end);

			_logger.LogInformation($"{results.Count} Matches for CreatorId: {creatorId}, Start: {start}, End: {end}");

			return results;
		}

		public List<Match> GetByGameAndCreator(int gameId, int creatorId)
		{
			var results = _matchDbController.GetByGameAndCreator(gameId, creatorId);

			_logger.LogInformation($"{results.Count} Matches for GameId: {gameId}, CreatorId: {creatorId}");

			return results;
		}

		public List<Match> GetByGameAndCreator(int gameId, int creatorId, DateTime? start, DateTime? end)
		{
			var results = _matchDbController.GetByGameAndCreator(gameId, creatorId, start, end);

			_logger.LogInformation($"{results.Count} Matches for GameId: {gameId}, CreatorId: {creatorId}, Start: {start}, End: {end}");

			return results;
		}

		public List<EvaluationData> GetData(int matchId, string[] keys = null)
		{
			return _evaluationDataController.Get(matchId, keys);
		}

		public EvaluationData AddData(EvaluationData newData)
		{
			ValidateData(newData);

			return _evaluationDataController.Add(newData);
		}

		private static void ValidateData(EvaluationData data)
		{
			if (data.MatchId == null)
			{
				throw new InvalidDataException("Cannot save Match data with no EntityId. EntityId needs to be set to the match's Id.");
			}
		}
	}
}