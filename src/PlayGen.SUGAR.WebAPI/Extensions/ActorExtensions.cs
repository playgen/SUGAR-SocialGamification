using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ActorExtensions
	{
		public static ActorResponse ToContract(this Actor actorModel)
		{
			if (actorModel == null)
			{
				return null;
			}

			return new ActorResponse
			{
				Id = actorModel.Id,
			};
		}
	}
}