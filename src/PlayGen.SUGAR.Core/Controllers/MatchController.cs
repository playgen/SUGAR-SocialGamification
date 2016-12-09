using System;
using NLog;
using PlayGen.SUGAR.Data.Model;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class MatchController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Data.EntityFramework.Controllers.MatchController _matchDbController;

        public MatchController(Data.EntityFramework.Controllers.MatchController matchDbController)
        {
            _matchDbController = matchDbController;
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
            match.Ended = DateTime.UtcNow;
            match = _matchDbController.Update(match);

            Logger.Info($"Match: {match.Id} ended");

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
    }
}
