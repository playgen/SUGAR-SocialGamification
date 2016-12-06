using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using System;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class EvaluationController : DbController
	{
		public EvaluationController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

        public List<Evaluation> Get()
        {
            using (var context = ContextFactory.Create())
            {
                return context.Evaluations
                    .IncludeAll()
                    .ToList();
            }
        }

        public List<Evaluation> GetByGame(int? gameId)
		{
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				var evaluations = context.Evaluations
					.IncludeAll()
					.Where(a => a.GameId == gameId).ToList();

				return evaluations;
			}
		}

		public Evaluation Get(string token, int? gameId)
		{
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				return context.Evaluations
					.IncludeAll()
					.SingleOrDefault(e => e.Token == token && e.GameId == gameId);
			}
		}

		public Evaluation Create(Evaluation evaluation)
		{
			using (var context = ContextFactory.Create())
			{
				//TODO: refine duplicate text for actor type and game id
				var hasConflicts = context.Evaluations.Any(a => (a.Name == evaluation.Name && a.GameId == evaluation.GameId)
									|| (a.Token == evaluation.Token && a.GameId == evaluation.GameId));

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"An evaluation with the name {evaluation.Name} or token {evaluation.Token} for this game already exists.");
				}

				context.Evaluations.Add(evaluation);
				SaveChanges(context);
				return evaluation;
			}
		}

		public void Update(Evaluation evaluation)
		{
			using (var context = ContextFactory.Create())
			{
			    context.Evaluations.Update(evaluation);
			    context.SaveChanges();
			}
		}

        public void Delete(string token, int? gameId)
		{
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;
				
				var evaluation = context.Evaluations
					.IncludeAll()
					.SingleOrDefault(e => e.Token == token && e.GameId == gameId);

				if (evaluation != null)
				{
					context.Evaluations.Remove(evaluation);
					SaveChanges(context);
				}
			}
		}
	}
}
