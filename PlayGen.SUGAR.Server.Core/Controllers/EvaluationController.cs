using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Extensions;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class EvaluationController : CriteriaEvaluator
	{
		public static Action<Evaluation> EvaluationCreatedEvent;
		public static Action<Evaluation> EvaluationUpdatedEvent;
		public static Action<Evaluation> EvaluationDeletedEvent;

		private readonly ILogger _logger;
		private readonly RewardController _rewardController;
		private readonly ActorController _actorController;
		private readonly EntityFramework.Controllers.EvaluationController _evaluationDbController;

		public EvaluationController(
			ILogger<EvaluationController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			EntityFramework.Controllers.EvaluationController evaluationDbController,
			RelationshipController relationshipCoreController,
			ActorController actorController,
			RewardController rewardController,
			SUGARContextFactory contextFactory)
			: base(evaluationDataLogger, contextFactory, relationshipCoreController)
		{
			_logger = logger;
			_evaluationDbController = evaluationDbController;
			_rewardController = rewardController;
			_actorController = actorController;
		}

		public List<Evaluation> Get()
		{
			var evaluations = _evaluationDbController.Get();

			_logger.LogInformation($"{evaluations?.Count}");

			return evaluations;
		}

		public List<Evaluation> GetByGame(int gameId)
		{
			var evaluations = _evaluationDbController.GetByGame(gameId);

			_logger.LogInformation($"{evaluations?.Count} Evalautions for GameId: {gameId}");

			return evaluations;
		}

		public List<Evaluation> GetByGame(int gameId, EvaluationType evaluationType)
		{
			var evaluations = _evaluationDbController.GetByGame(gameId, evaluationType);

			_logger.LogInformation($"{evaluations?.Count} Evalautions for GameId: {gameId}");

			return evaluations;
		}

		public Evaluation Get(string token, int gameId, EvaluationType evaluationType)
		{
			var evaluation = _evaluationDbController.Get(token, gameId, evaluationType);

			_logger.LogInformation($"Evalaution: {evaluation?.Id} for Token: {token}, GameId: {gameId}");

			return evaluation;
		}

		public List<EvaluationProgress> GetGameProgress(int gameId, int actorId, EvaluationType evaluationType)
		{
			var evaluations = GetByGame(gameId, evaluationType);
			evaluations = FilterByActorType(evaluations, actorId);

			var evaluationsProgress = evaluations.Select(e => GetProgress(e.Token, gameId, actorId, evaluationType)).ToList();

			_logger.LogInformation($"{evaluationsProgress.Count} Evaluation Progresses for GameId: {gameId}, ActorId: {actorId}");

			return evaluationsProgress;
		}

		public EvaluationProgress GetProgress(string token, int gameId, int actorId, EvaluationType evaluationType)
		{
			var evaluation = Get(token, gameId, evaluationType);
			var progress = EvaluateProgress(evaluation, actorId);

			var result = new EvaluationProgress {
				Actor = _actorController.Get(actorId),
				Evaluation = evaluation,
				Progress = progress
			};

			_logger.LogInformation($"{result.Evaluation.Name} Evaluation Progresses for Token: {token}, GameId: {gameId}, ActorId: {actorId}");

			return result;
		}

		public Evaluation Create(Evaluation evaluation)
		{
			foreach (var ec in evaluation.EvaluationCriterias)
			{
				if (!DataTypeValueValidation(ec.EvaluationDataType, ec.Value))
				{
					throw new InvalidCastException($"{ec.Value} cannot be cast to DataType {ec.EvaluationDataType}");
				}
			}
			evaluation = _evaluationDbController.Create(evaluation);

			EvaluationCreatedEvent?.Invoke(evaluation);

			_logger.LogInformation($"{evaluation?.Id}");

			return evaluation;
		}

		public void Update(Evaluation evaluation)
		{
			foreach (var ec in evaluation.EvaluationCriterias)
			{
				if (!DataTypeValueValidation(ec.EvaluationDataType, ec.Value))
				{
					throw new InvalidCastException($"{ec.Value} cannot be cast to DataType {ec.EvaluationDataType}");
				}
			}
			_evaluationDbController.Update(evaluation);

			_logger.LogInformation($"{evaluation.Id}");

			EvaluationUpdatedEvent?.Invoke(evaluation);
		}

		public void Delete(string token, int gameId, EvaluationType evaluationType)
		{
			var evaluation = Get(token, gameId, evaluationType);

			if (evaluation == null)
			{
				throw new MissingRecordException($"The evaluation with token \"{token}\" for gameId {gameId} cannot be found.");
			}

			EvaluationDeletedEvent?.Invoke(evaluation);
			_evaluationDbController.Delete(token, gameId, evaluationType);

			_logger.LogInformation($"Deleted: {evaluation.Id} for Token {token}, GameId: {gameId}");
		}

		private bool DataTypeValueValidation(EvaluationDataType dataType, string value)
		{
			switch (dataType)
			{
				case EvaluationDataType.String:
					return true;
				case EvaluationDataType.Long:
					return long.TryParse(value, out var _);
				case EvaluationDataType.Float:
					return float.TryParse(value, out var _);
				case EvaluationDataType.Boolean:
					return bool.TryParse(value, out var _);
				default:
					return false;
			}
		}

		public float EvaluateProgress(Evaluation evaluation, int actorId)
		{
			if (evaluation == null)
			{
				throw new MissingRecordException("The provided evaluation does not exist.");
			}
			var provided = _actorController.Get(actorId);
			if (evaluation.ActorType != ActorType.Undefined && (provided == null || provided.ActorType != evaluation.ActorType))
			{
				throw new MissingRecordException("The provided ActorId cannot complete this evaluation.");
			}

			var completed = IsAlreadyCompleted(evaluation, actorId);
			var completedProgress = completed ? 1f : 0f;

			if (!completed)
			{
				completedProgress = IsCriteriaSatisified(evaluation.GameId, actorId, evaluation.EvaluationCriterias, evaluation.ActorType, evaluation.EvaluationType);
				if (completedProgress >= 1)
				{
					SetCompleted(evaluation, actorId);
				}
			}

			_logger.LogDebug($"Got: Progress: {completedProgress} for Evaluation.Id: {evaluation.Id}, ActorId: {actorId}");

			return completedProgress;
		}

		public bool IsAlreadyCompleted(Evaluation evaluation, int actorId)
		{
			var evaluationDataCoreController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, evaluation.EvaluationType.ToEvaluationDataCategory());

			var key = evaluation.Token;
			var completed = evaluationDataCoreController.KeyExists(evaluation.GameId, actorId, key, EvaluationDataType.String);

			_logger.LogDebug($"Got: IsCompleted: {completed} for Evaluation.Id: {evaluation.Id}, ActorId: {actorId}");

			return completed;
		}

		private void SetCompleted(Evaluation evaluation, int actorId)
		{
			var evaluationDataCoreController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, evaluation.EvaluationType.ToEvaluationDataCategory());

			var evaluationData = new EvaluationData {
				Category = evaluation.EvaluationType.ToEvaluationDataCategory(),
				Key = evaluation.Token,
				GameId = evaluation.GameId,
				ActorId = actorId,
				EvaluationDataType = EvaluationDataType.String,
				Value = null
			};

			evaluationDataCoreController.Add(evaluationData);

			ProcessEvaluationRewards(evaluation, actorId);
		}

		private void ProcessEvaluationRewards(Evaluation evaluation, int actorId)
		{
			evaluation.Rewards?.ForEach(reward => _rewardController.AddReward(actorId, evaluation.GameId, reward));
		}

		private List<Evaluation> FilterByActorType(List<Evaluation> evaluations, int actorId)
		{
			var provided = _actorController.Get(actorId);
			evaluations = provided == null
				? evaluations.Where(a => a.ActorType == ActorType.Undefined).ToList()
				: evaluations.Where(a => a.ActorType == ActorType.Undefined || a.ActorType == provided.ActorType).ToList();

			return evaluations;
		}
	}
}