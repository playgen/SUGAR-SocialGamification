using System.Collections.Generic;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;
using System.Linq;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
    public static class MatchExtensions
    {
        public static List<MatchResponse> ToContractList(this List<Match> models)
        {
            return models?.Select(ToContract).ToList();
        }

        public static MatchResponse ToContract(this Match model)
        {
            return new MatchResponse
            {
                Id = model.Id,
                Game = model.Game?.ToContract(),
                Creator = model.Creator?.ToContract(),
                Started = model.Started,
                Ended = model.Ended
            };
        }
    }
}
