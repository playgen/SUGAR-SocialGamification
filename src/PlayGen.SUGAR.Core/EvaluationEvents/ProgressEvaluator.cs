// todo v2.0: support group evaluations
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

namespace PlayGen.SUGAR.Core.EvaluationEvents
{
	/// <summary>
	/// Evaluation progress on actor per game basis for every actor that has an active session.
	/// </summary>
	public class ProgressEvaluator
	{
		private readonly ProgressCache _progressCache = new ProgressCache();
		private readonly CriteriaEvaluator _evaluationCriteriaEvaluator;

		public ProgressEvaluator(CriteriaEvaluator evaluationCriteriaEvaluator)
		{
			_evaluationCriteriaEvaluator = evaluationCriteriaEvaluator;
		}
		
		public void RemoveActor(int? gameId, int actorId)
		{
			// todo remove progress for user for game
			throw new NotImplementedException();
		}

		// <evaluation, progress>
		public Dictionary<Evaluation, float> EvaluateActor(IEnumerable<Evaluation> evaluations, int? gameId, Actor actor)
		{
		    foreach (var evaluation in evaluations)
		    {
                var progress = _evaluationCriteriaEvaluator.IsCriteriaSatisified(gameId, actor.Id, evaluation.EvaluationCriterias, actor.ActorType);
		        _progressCache.AddProgress(gameId, actor.Id, evaluation, progress);
		    }

		    var actorProgress = _progressCache.GetActorProgress(gameId, actor.Id);
            return actorProgress;
		}

		// <gameId, <actorId, <evaluation, progress>>>
		public Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> Evaluate(Evaluation evaluation)
		{
			var affectedActors = GetAffectedActors(evaluation);

			foreach (var actorId in affectedActors)
			{
				EvaluateActor(evaluation, actorId);
			}

			throw new NotImplementedException();
		}

		// <gameId, <actorId, <evaluation, progress>>>
		public Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> Evaluate(IEnumerable<Evaluation> evaluations, GameData gameData)
		{
			var affectedActorsByEvaluation = GetAffectedActors(evaluations, gameData);

			foreach (var evaluation in affectedActorsByEvaluation.Keys)
			{
				foreach (var actorId in affectedActorsByEvaluation[evaluation])
				{
					EvaluateActor(evaluation, actorId);
				}
			}

			throw new NotImplementedException();
		}

		public void Remove(Evaluation evaluation)
		{
			// todo remove all progress for this evaluation
			throw new NotImplementedException();
		}

		// <gameId, <actorId, <evaluation, progress>>>
		private Dictionary<int, Dictionary<int, Dictionary<Evaluation, float>>> EvaluateActor(Evaluation evaluation, int actorId)
		{
			// todo evaluate against te actor
			throw new NotImplementedException();
		}

		private Dictionary<Evaluation, List<int>> GetAffectedActors(IEnumerable<Evaluation> evaluations, GameData gameData)
		{
			var affectedActorsByEvaluation = new Dictionary<Evaluation, List<int>>();

			foreach (var evaluation in evaluations)
			{
				affectedActorsByEvaluation[evaluation] = GetAffectedActors(evaluation, gameData);
			}

			return affectedActorsByEvaluation;
		}

		private List<int> GetAffectedActors(Evaluation evaluation)
		{
			// todo based on evaluation actor type and scope
			return _progressCache.GetActors(evaluation.GameId);
		}

		private List<int> GetAffectedActors(Evaluation evaluation, GameData gameData)
		{
			// todo based on evaluation actor type, scope and game data id determine which actors are affected for each evaluation
			// todo make sure actor id is in the tracked actor id list
			return new List<int>() { gameData.ActorId.Value }; // 
		}
	}
}
