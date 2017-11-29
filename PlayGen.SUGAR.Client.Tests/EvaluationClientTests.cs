using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.Tests
{
	public abstract class EvaluationClientTests : ClientTestBase
	{
		protected void CompleteGenericEvaluation(string key, int userId, int gameId = 0)
		{
			SUGARClient.GameData.Add(new EvaluationDataRequest
			{
				CreatingActorId = userId,
				EvaluationDataType = EvaluationDataType.Long,
				Value = $"{200}",
				Key = key,
				GameId = gameId
			});
		}
	}
}
