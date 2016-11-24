using System.Collections.Generic;
using PlayGen.SUGAR.Contracts.Shared;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
	public static class ActorExtensions
	{
        public static IEnumerable<ActorResponse> ToContractList(this IEnumerable<Actor> actorModels)
        {
            return actorModels.Select(ToContract).ToList();
        }

        public static ActorResponse ToContract(this Actor model)
	    {
	        return new ActorResponse
	        {
	            Id = model.Id,

	            Name = model.Name
	        };
	    }
	}
}