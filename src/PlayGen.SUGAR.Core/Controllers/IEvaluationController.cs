using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.Model;
using EvaluationCriteria = PlayGen.SUGAR.Data.Model.EvaluationCriteria;

namespace PlayGen.SUGAR.Core.Controllers
{
	public interface IEvaluationController
	{
		List<Evaluation> Get();
		List<Evaluation> GetByGame(int? gameId);
		Evaluation Get(string token, int? gameId);
		List<EvaluationProgress> GetGameProgress(int gameId, int? actorId);
		EvaluationProgress GetProgress(string token, int? gameId, int actorId);
		Evaluation Create(Evaluation evaluation);
		void Update(Evaluation evaluation);
		void Delete(string token, int? gameId);
		float EvaluateProgress(Evaluation evaluation, int? actorId);
		bool IsAlreadyCompleted(Evaluation evaluation, int actorId);
		float IsCriteriaSatisified(int? gameId, int? actorId, List<EvaluationCriteria> completionCriterias, ActorType actorType);
	}
}