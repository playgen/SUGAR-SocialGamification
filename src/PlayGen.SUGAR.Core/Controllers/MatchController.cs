using System;
using NLog;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework;
using PlayGen.SUGAR.Core.Exceptions;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class MatchController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Data.EntityFramework.Controllers.MatchController _matchDbController;
        private readonly EvaluationDataController _evaluationDataController;
        
        public MatchController(SUGARContextFactory contextFactory, Data.EntityFramework.Controllers.MatchController matchDbController)
        {
            _matchDbController = matchDbController;
            _evaluationDataController = new EvaluationDataController(contextFactory, EvaluationDataCategory.MatchData);
        }

        public Match Create(int gameId, int creatorId)
        {
            var match = new Match
            {
                GameId = gameId,
                CreatorId = creatorId,
            };

            _matchDbController.Create(match);

            Logger.Info($"Match: {match.Id} created for Game: {gameId}, CreatorId: {creatorId}");

            return match;
        }

        public Match Start(int matchId)
        {
            var match = _matchDbController.Get(matchId);
            match.Started = DateTime.UtcNow;
            match = _matchDbController.Update(match);

            Logger.Info($"Match: {match.Id} started");

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

            Logger.Info($"Match: {match.Id} ended");

            return match;
        }

        public Match Get(int matchId)
        {
            var match = _matchDbController.Get(matchId);

            Logger.Info($"Found {match?.Id}");

            return match;
        }

        public List<Match> GetByTime(DateTime? start, DateTime? end)
        {
            var results = _matchDbController.GetByTime(start, end);

            Logger.Info($"{results.Count} Matches for Start: {start}, End: {end}");

            return results;
        }

        public List<Match> GetByGame(int gameId)
        {
            var results = _matchDbController.GetByGame(gameId);

            Logger.Info($"{results.Count} Matches for GameId: {gameId}");

            return results;
        }

        public List<Match> GetByGame(int gameId, DateTime? start, DateTime? end)
        {
            var results = _matchDbController.GetByGame(gameId, start, end);

            Logger.Info($"{results.Count} Matches for GameId: {gameId}, Start: {start}, End: {end}");

            return results;
        }

        public List<Match> GetByCreator(int creatorId)
        {
            var results = _matchDbController.GetByCreator(creatorId);

            Logger.Info($"{results.Count} Matches for Creator: {creatorId}");

            return results;
        }

        public List<Match> GetByCreator(int creatorId, DateTime? start, DateTime? end)
        {
            var results = _matchDbController.GetByCreator(creatorId, start, end);

            Logger.Info($"{results.Count} Matches for CreatorId: {creatorId}, Start: {start}, End: {end}");

            return results;
        }

        public List<Match> GetByGameAndCreator(int gameId, int creatorId)
        {
            var results = _matchDbController.GetByGameAndCreator(gameId, creatorId);

            Logger.Info($"{results.Count} Matches for GameId: {gameId}, CreatorId: {creatorId}");

            return results;
        }

        public List<Match> GetByGameAndCreator(int gameId, int creatorId, DateTime? start, DateTime? end)
        {
            var results = _matchDbController.GetByGameAndCreator(gameId, creatorId, start, end);

            Logger.Info($"{results.Count} Matches for GameId: {gameId}, CreatorId: {creatorId}, Start: {start}, End: {end}");

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