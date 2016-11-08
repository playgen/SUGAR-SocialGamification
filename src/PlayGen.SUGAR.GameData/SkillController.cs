using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;


namespace PlayGen.SUGAR.GameData
{
	public class SkillController : DataEvaluationController
	{
		protected readonly RewardController RewardController;
		protected readonly ActorController ActorController;

		public SkillController(GameDataController gameDataController,
			GroupRelationshipController groupRelationshipController,
			UserRelationshipController userRelationshipController,
			ActorController actorController,
			RewardController rewardController)
			: base(gameDataController, groupRelationshipController, userRelationshipController)
		{
			RewardController = rewardController;
			ActorController = actorController;
		}

		public IEnumerable<Skill> FilterByActorType(IEnumerable<Skill> skills, int? actorId)
		{
			if (actorId.HasValue)
			{
				var provided = ActorController.Get(actorId.Value);
				if (provided == null)
				{
					skills = skills.Where(a => a.ActorType == ActorType.Undefined);
				}
				else
				{
					skills = skills.Where(a => a.ActorType == ActorType.Undefined || a.ActorType == provided.ActorType);
				}
			}

			return skills;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="actorId"></param>
		/// <returns></returns>
		public float IsSkillCompleted(Skill skill, int? actorId)
		{
			if (skill == null)
			{
				throw new MissingRecordException("The provided skill does not exist.");
			}
			if (actorId != null)
			{
				var provided = ActorController.Get(actorId.Value);
				if (skill.ActorType != ActorType.Undefined && (provided == null || provided.ActorType != skill.ActorType))
				{
					throw new MissingRecordException("The provided ActorId cannot complete this skill.");
				}
			}
			var key = string.Format(KeyConstants.SkillCompleteFormat, skill.Token);
			var completed = GameDataController.KeyExists(skill.GameId, actorId, key);
			var completedProgress = completed ? 1f : 0f;
			if (!completed)
			{
				completedProgress = IsCriteriaSatisified(skill.GameId, actorId, skill.CompletionCriterias, skill.ActorType);
				if (completedProgress >= 1)
				{
					ProcessSkillRewards(skill, actorId);
				}
			}

			return completedProgress;
		}

		private void ProcessSkillRewards(Skill skill, int? actorId)
		{
			var gameData = new Data.Model.GameData()
			{
				Key = string.Format(KeyConstants.SkillCompleteFormat, skill.Token),
				GameId = skill.GameId,    //TODO: handle the case where a global skill has been completed for a specific game
				ActorId = actorId,
				DataType = GameDataType.String,
				Value = null
			};
			GameDataController.Create(gameData);
			skill.Rewards.All(reward => RewardController.AddReward(actorId, skill.GameId, reward));
		}

		public void EvaluateSkill(Skill skill, int? actorId)
		{
		}
	}
}