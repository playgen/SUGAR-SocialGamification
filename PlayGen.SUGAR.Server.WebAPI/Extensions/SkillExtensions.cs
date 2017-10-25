using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.WebAPI.Extensions
{
	public static class SkillExtensions
	{
		public static EvaluationResponse ToContract(this Skill model)
		{
			if (model == null)
			{
				return null;
			}

			return new EvaluationResponse {
				Id = model.Id,
				Name = model.Name,
				Description = model.Description,
				GameId = model.GameId,
				ActorType = model.ActorType,
				Token = model.Token,
				EvaluationCriterias = model.EvaluationCriterias.ToContractList(),
				Rewards = model.Rewards.ToContractList(),
			};
		}

		public static IEnumerable<EvaluationResponse> ToContractList(this IEnumerable<Skill> models)
		{
			return models.Select(ToContract).ToList();
		}

		public static Skill ToSkillModel(this EvaluationCreateRequest skillContract)
		{
			var skillModel = new Skill {
				Name = skillContract.Name,
				Description = skillContract.Description,
				GameId = skillContract.GameId ?? 0,
				ActorType = skillContract.ActorType,
				Token = skillContract.Token,
				EvaluationCriterias = skillContract.EvaluationCriterias.ToModelList(),
				Rewards = skillContract.Rewards.ToModelList(),
			};

			return skillModel;
		}

		public static Skill ToSkillModel(this EvaluationUpdateRequest skillContract)
		{
			var skillModel = new Skill {
				Id = skillContract.Id,
				Name = skillContract.Name,
				Description = skillContract.Description,
				GameId = skillContract.GameId ?? 0,
				ActorType = skillContract.ActorType,
				Token = skillContract.Token,
				EvaluationCriterias = skillContract.EvaluationCriterias.ToModelList(),
				Rewards = skillContract.Rewards.ToModelList(),
			};

			return skillModel;
		}
	}
}