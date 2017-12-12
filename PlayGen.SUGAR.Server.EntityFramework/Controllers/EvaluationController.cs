using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;
using PlayGen.SUGAR.Server.Model;
using System.Collections.Generic;
using System.Linq;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
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

		public List<Evaluation> GetByGame(int gameId)
		{
			using (var context = ContextFactory.Create())
			{
				var evaluations = context.Evaluations
					.IncludeAll()
					.Where(a => a.GameId == gameId)
					.ToList();

				return evaluations;
			}
		}

		public List<Evaluation> GetByGame(int gameId, EvaluationType evaluationType)
		{
			using (var context = ContextFactory.Create())
			{
				var evaluations = context.Evaluations
					.IncludeAll()
					.Where(a => a.GameId == gameId && a.EvaluationType == evaluationType)
					.ToList();

				return evaluations;
			}
		}

		public Evaluation Get(string token, int gameId, EvaluationType evaluationType)
		{
			using (var context = ContextFactory.Create())
			{
				return context.Evaluations
					.IncludeAll()
					.SingleOrDefault(e => e.Token == token && e.GameId == gameId && e.EvaluationType == evaluationType);
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
				SaveChanges(context);
			}
		}

		public void Delete(string token, int gameId, EvaluationType evaluationType)
		{
			using (var context = ContextFactory.Create())
			{
				var evaluation = context.Evaluations
					.IncludeAll()
					.SingleOrDefault(e => e.Token == token && e.GameId == gameId && e.EvaluationType == evaluationType);

				if (evaluation == null)
				{
					throw new MissingRecordException($"No Evaluation exists with Token: {token}, GameId: {gameId} and EvaluationType: {evaluationType}");
				}
				context.Evaluations.Remove(evaluation);
				SaveChanges(context);
			}
		}
	}
}
