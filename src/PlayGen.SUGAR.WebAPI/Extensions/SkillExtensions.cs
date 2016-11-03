using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class SkillExtensions
	{
		public static AchievementResponse ToContract(this Skill model)
		{
			if (model == null)
			{
				return null;
			}

            return new AchievementResponse
			{
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				CompletionCriterias = model.CompletionCriterias.ToContractList(),
				Rewards = model.Rewards.ToContractList(),
			};
		}

		public static IEnumerable<AchievementResponse> ToContractList(this IEnumerable<Skill> models)
		{
			return models.Select(ToContract).ToList();
		}
        
        public static Skill ToSkillModel(this AchievementRequest skillContract)
        {
            var skillModel = new Skill
            {
                Name = skillContract.Name,
                Description = skillContract.Description,
                GameId = skillContract.GameId ?? 0,
                ActorType = skillContract.ActorType,
                Token = skillContract.Token,
                CompletionCriterias = skillContract.CompletionCriterias.ToModelList(),
                Rewards = skillContract.Rewards.ToModelList(),
            };

            return skillModel;
        }
    }
}