using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class SkillExtensions
	{
		public static EvaluationResponse ToContract(this Skill model)
		{
			if (model == null)
				return null;

			return new EvaluationResponse
			{
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				EvaluationCriterias = model.EvaluationCriterias.Select(ec => ec.ToContract()).ToList(),
				Rewards = model.Rewards.Select(r => r.ToContract()).ToList(),
			};
		}

		public static EvaluationsResponse ToCollectionContract(this IEnumerable<Skill> models)
		{
			return new EvaluationsResponse() {
				Items = models.Select(ToContract).ToArray(),
			};
		}

		public static Skill ToSkillModel(this EvaluationCreateRequest skillContract)
		{
			var skillModel = new Skill
			{
				Name = skillContract.Name,
				Description = skillContract.Description,
				GameId = skillContract.GameId ?? 0,
				ActorType = skillContract.ActorType,
				Token = skillContract.Token,
				EvaluationCriterias = skillContract.EvaluationCriterias.ToModelList(),
				Rewards = skillContract.Rewards.ToModelList()
			};

			return skillModel;
		}

		public static Skill ToSkillModel(this EvaluationUpdateRequest skillContract)
		{
			var skillModel = new Skill
			{
				Id = skillContract.Id,
				Name = skillContract.Name,
				Description = skillContract.Description,
				GameId = skillContract.GameId ?? 0,
				ActorType = skillContract.ActorType,
				Token = skillContract.Token,
				EvaluationCriterias = skillContract.EvaluationCriterias.ToModelList(),
				Rewards = skillContract.Rewards.ToModelList()
			};

			return skillModel;
		}
	}
}