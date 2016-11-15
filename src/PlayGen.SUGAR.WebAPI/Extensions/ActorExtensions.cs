using System.Collections.Generic;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

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

        public static IEnumerable<ActorResponse> ToContractList(this IEnumerable<Actor> actorModels)
        {
            return actorModels.Select(ToContract).ToList();
        }

        public static ActorResponse ToContract(this Common.Shared.Actor model)
	    {
	        return new ActorResponse
	        {
	            Id = model.Id,

	            Name = model.Name
	        };
	    }
	}
}