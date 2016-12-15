using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class SkillClientTests : Evaluations
	{
		[Test]
		public void CanCreateSkill()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanCreateSkill",
				ActorType = ActorType.User,
				Token = "CanCreateSkill",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanCreateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			Assert.AreEqual(skillRequest.Token, response.Token);
			Assert.AreEqual(skillRequest.ActorType, response.ActorType);
		}

		public void CanCreateGlobalSkill()
		{
			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanCreateGlobalSkill",
				ActorType = ActorType.User,
				Token = "CanCreateGlobalSkill",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanCreateGlobalSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			Assert.AreEqual(skillRequest.Token, response.Token);
			Assert.AreEqual(skillRequest.ActorType, response.ActorType);
		}

		[Test]
		public void CannotCreateDuplicateSkill()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateDuplicateSkill",
				ActorType = ActorType.User,
				Token = "CannotCreateDuplicateSkill",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotCreateDuplicateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			SUGARClient.Skill.Create(skillRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Create(skillRequest));
		}

		[Test]
		public void CannotCreateSkillWithNoName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoName",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotCreateSkillWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Create(skillRequest));
		}

		[Test]
		public void CannotCreateSkillWithNoToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateSkillWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotCreateSkillWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Create(skillRequest));
		}

		[Test]
		public void CannotCreateSkillWithNoEvaluationCriteria()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateSkillWithNoEvaluationCriteria",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoEvaluationCriteria",
				GameId = game.Id,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Create(skillRequest));
		}

		[Test]
		public void CannotCreateSkillWithNoEvaluationCriteriaKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateSkillWithNoEvaluationCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Create(skillRequest));
		}

		[Test]
		public void CannotCreateSkillWithNoEvaluationCriteriaValue()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateSkillWithNoEvaluationCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotCreateSkillWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Create(skillRequest));
		}

		[Test]
		public void CannotCreateSkillWithNoEvaluationCriteriaDataTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotCreateSkillWithNoEvaluationCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotCreateSkillWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotCreateSkillWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Create(skillRequest));
		}

		[Test]
		public void CanGetSkillsByGame()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "GameGet");

			var skillRequestOne = new EvaluationCreateRequest()
			{
				Name = "CanGetSkillsByGameOne",
				ActorType = ActorType.User,
				Token = "CanGetSkillsByGameOne",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetSkillsByGameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseOne = SUGARClient.Skill.Create(skillRequestOne);

			var skillRequestTwo = new EvaluationCreateRequest()
			{
				Name = "CanGetSkillsByGameTwo",
				ActorType = ActorType.User,
				Token = "CanGetSkillsByGameTwo",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetSkillsByGameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseTwo = SUGARClient.Skill.Create(skillRequestTwo);

			var getSkill = SUGARClient.Skill.GetByGame(game.Id);

			Assert.AreEqual(2, getSkill.Count());
		}

		[Test]
		public void CanGetSkillByKeys()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetSkillByKeys",
				ActorType = ActorType.User,
				Token = "CanGetSkillByKeys",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetSkillByKeys",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var getSkill = SUGARClient.Skill.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.AreEqual(response.Name, getSkill.Name);
			Assert.AreEqual(skillRequest.Name, getSkill.Name);
		}

		[Test]
		public void CannotGetNotExistingSkillByKeys()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var getSkill = SUGARClient.Skill.GetById("CannotGetNotExistingSkillByKeys", game.Id);

			Assert.Null(getSkill);
		}

		[Test]
		public void CannotGetSkillByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			Assert.Throws<ClientException>(() => SUGARClient.Skill.GetById("", game.Id));
		}
        
		[Test]
		public void CanGetGlobalSkillByToken()
		{
			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetGlobalSkillByToken",
				ActorType = ActorType.User,
				Token = "CanGetGlobalSkillByToken",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetGlobalSkillByToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var getSkill = SUGARClient.Skill.GetGlobalById(skillRequest.Token);

			Assert.AreEqual(response.Name, getSkill.Name);
			Assert.AreEqual(skillRequest.Name, getSkill.Name);
		}

		[Test]
		public void CannotGetNotExistingGlobalSkillByKeys()
		{
			var getSkill = SUGARClient.Skill.GetGlobalById("CannotGetNotExistingGlobalSkillByKeys");

			Assert.Null(getSkill);
		}

		[Test]
		public void CannotGetGlobalSkillByEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Skill.GetGlobalById(""));
		}
        
		[Test]
		public void CannotGetBySkillsByNotExistingGameId()
		{
			var getSkills = SUGARClient.Skill.GetByGame(-1);

			Assert.IsEmpty(getSkills);
		}

		[Test]
		public void CanUpdateSkill()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanUpdateSkill",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateSkill",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanUpdateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CanUpdateSkill Updated",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CanUpdateSkill",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
						Key = "CanUpdateSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			SUGARClient.Skill.Update(updateRequest);

			var updateResponse = SUGARClient.Skill.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.AreNotEqual(response.Name, updateResponse.Name);
			Assert.AreEqual("CanUpdateSkill Updated", updateResponse.Name);
		}

		[Test]
		public void CannotUpdateSkillToDuplicateToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequestOne = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateSkillToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillToDuplicateNameOne",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateSkillToDuplicateNameOne",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseOne = SUGARClient.Skill.Create(skillRequestOne);

			var skillRequestTwo = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateSkillToDuplicateNameTwo",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillToDuplicateNameTwo",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateSkillToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var responseTwo = SUGARClient.Skill.Create(skillRequestTwo);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = responseTwo.Id,
				Name = "CannotUpdateSkillToDuplicateNameOne",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillToDuplicateNameOne",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = responseTwo.EvaluationCriterias[0].Id,
						Key = "CannotUpdateSkillToDuplicateNameTwo",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateNonExistingSkill()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = int.MaxValue,
				Name = "CannotUpdateNonExistingSkill",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateNonExistingSkill",
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = int.MaxValue,
						Key = "CannotUpdateNonExistingSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateAchievemenWithNoName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateAchievemenWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateAchievemenWithNoName",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
						Key = "CannotUpdateAchievemenWithNoName",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateSkillWithNoToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateSkillWithNoToken",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoToken",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateSkillWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateSkillWithNoToken",
				ActorType = ActorType.User,
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
						Key = "CannotUpdateSkillWithNoToken",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateSkillWithNoEvaluationCriteria()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateSkillWithNoEvaluationCriteria",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteria",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateSkillWithNoEvaluationCriteria",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateSkillWithNoEvaluationCriteria",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteria",
				GameId = game.Id,
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateSkillWithNoEvaluationCriteriaKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateSkillWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteriaKey",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateSkillWithNoEvaluationCriteriaKey",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateSkillWithNoEvaluationCriteriaKey",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteriaKey",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateSkillWithNoEvaluationCriteriaValue()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateSkillWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteriaValue",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateSkillWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateSkillWithNoEvaluationCriteriaValue",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteriaValue",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
						Key = "CannotUpdateSkillWithNoEvaluationCriteriaValue",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateSkillWithNoEvaluationCriteriaDataTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CannotUpdateSkillWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteriaDataTypeMismatch",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CannotUpdateSkillWithNoEvaluationCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var updateRequest = new EvaluationUpdateRequest()
			{
                Id = response.Id,
				Name = "CannotUpdateSkillWithNoEvaluationCriteriaDataTypeMismatch",
				ActorType = ActorType.User,
				Token = "CannotUpdateSkillWithNoEvaluationCriteriaDataTypeMismatch",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaUpdateRequest>()
				{
					new EvaluationCriteriaUpdateRequest()
					{
                        Id = response.EvaluationCriterias[0].Id,
						Key = "CannotUpdateSkillWithNoEvaluationCriteriaDataTypeMismatch",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "A string"
					}
				},
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Update(updateRequest));
		}

		[Test]
		public void CanDeleteSkill()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanDeleteSkill",
				ActorType = ActorType.User,
				Token = "CanDeleteSkill",
				GameId = game.Id,
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanDeleteSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var getSkill = SUGARClient.Skill.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.NotNull(getSkill);

			SUGARClient.Skill.Delete(skillRequest.Token, skillRequest.GameId.Value);

			getSkill = SUGARClient.Skill.GetById(skillRequest.Token, skillRequest.GameId.Value);

			Assert.Null(getSkill);
		}

		[Test]
		public void CannotDeleteNonExistingSkill()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.Delete("CannotDeleteNonExistingSkill", game.Id));
		}

		[Test]
		public void CannotDeleteSkillByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			Assert.Throws<ClientException>(() => SUGARClient.Skill.Delete("", game.Id));
		}

		[Test]
		public void CanDeleteGlobalSkill()
		{
			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanDeleteGlobalSkill",
				ActorType = ActorType.User,
				Token = "CanDeleteGlobalSkill",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanDeleteGlobalSkill",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"

					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var getSkill = SUGARClient.Skill.GetGlobalById(skillRequest.Token);

			Assert.NotNull(getSkill);

			SUGARClient.Skill.DeleteGlobal(skillRequest.Token);

			getSkill = SUGARClient.Skill.GetGlobalById(skillRequest.Token);

			Assert.Null(getSkill);
		}

		[Test]
		public void CannotDeleteNonExistingGlobalSkill()
		{
			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.DeleteGlobal("CannotDeleteNonExistingGlobalSkill"));
		}

		[Test]
		public void CannotDeleteGlobalSkillByEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Skill.DeleteGlobal(""));
		}

		[Test]
		public void CanGetGlobalSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetGlobalSkillProgress",
				ActorType = ActorType.Undefined,
				Token = "CanGetGlobalSkillProgress",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetGlobalSkillProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var progressGame = SUGARClient.Skill.GetGlobalProgress(user.Id);
			Assert.GreaterOrEqual(progressGame.Count(), 1);

			var progressSkill = SUGARClient.Skill.GetGlobalSkillProgress(response.Token, user.Id);
			Assert.AreEqual(0, progressSkill.Progress);

			var gameData = new EvaluationDataRequest()
			{
				Key = "CanGetGlobalSkillProgress",
				Value = "1",
				ActorId = user.Id,
				SaveDataType = SaveDataType.Float
			};

			SUGARClient.EvaluationData.Add(gameData);

			progressSkill = SUGARClient.Skill.GetGlobalSkillProgress(response.Token, user.Id);
			Assert.GreaterOrEqual(progressSkill.Progress, 1);
		}

		[Test]
		public void CannotGetNotExistingGlobalSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.GetGlobalSkillProgress("CannotGetNotExistingGlobalSkillProgress", user.Id));
		}

		[Test]
		public void CanGetSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "ProgressGet");

			var skillRequest = new EvaluationCreateRequest()
			{
				Name = "CanGetSkillProgress",
				GameId = game.Id,
				ActorType = ActorType.Undefined,
				Token = "CanGetSkillProgress",
				EvaluationCriterias = new List<EvaluationCriteriaCreateRequest>()
				{
					new EvaluationCriteriaCreateRequest()
					{
						Key = "CanGetSkillProgress",
						ComparisonType = ComparisonType.Equals,
						CriteriaQueryType = CriteriaQueryType.Any,
						DataType = SaveDataType.Float,
						Scope = CriteriaScope.Actor,
						Value = "1"
					}
				},
			};

			var response = SUGARClient.Skill.Create(skillRequest);

			var progressGame = SUGARClient.Skill.GetGameProgress(game.Id, user.Id);
			Assert.AreEqual(1, progressGame.Count());

			var progressSkill = SUGARClient.Skill.GetSkillProgress(response.Token, game.Id, user.Id);
			Assert.AreEqual(0, progressSkill.Progress);

			var gameData = new EvaluationDataRequest()
			{
				Key = "CanGetSkillProgress",
				Value = "1",
				ActorId = user.Id,
				GameId = game.Id,
				SaveDataType = SaveDataType.Float
			};

			SUGARClient.EvaluationData.Add(gameData);

			progressSkill = SUGARClient.Skill.GetSkillProgress(response.Token, game.Id, user.Id);
			Assert.AreEqual(1, progressSkill.Progress);
		}

		[Test]
		public void CannotGetNotExistingSkillProgress()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "ProgressGet");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "ProgressGet");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Skill.GetSkillProgress("CannotGetNotExistingSkillProgress", game.Id, user.Id));
		}

        #region Helpers
        protected override EvaluationResponse CreateEvaluation(EvaluationCreateRequest skillRequest)
        {
            var getSkill = SUGARClient.Skill.GetById(skillRequest.Token, skillRequest.GameId ?? 0);

            if (getSkill != null)
            {
                if (skillRequest.GameId.HasValue)
                {
                    SUGARClient.Skill.Delete(skillRequest.Token, skillRequest.GameId.Value);
                }
                else
                {
                    SUGARClient.Skill.DeleteGlobal(skillRequest.Token);
                }
            }

            var response = SUGARClient.Skill.Create(skillRequest);

            return response;
        }
        #endregion
    }
}
