using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.WebAPI.Extensions
{
    public static class MatchExtensions
    {
        public static MatchResponse ToContract(this Match model)
        {
            return new MatchResponse
            {
                Id = model.Id,
                Game = model.Game.ToContract(),
                Creator = model.Creator.ToContract(),
                Started = model.Started,
                Ended = model.Ended
            };
        }
    }
}
