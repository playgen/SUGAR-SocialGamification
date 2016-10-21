using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlayGen.SUGAR.Contracts
{
    public class AchievementTestRequest : IRequest
    {
		[Required]
	    public AchievementRequest Achievement { get; set; }

		[Required]
		public List<GameDataRequest> GameDatas { get; set; }
    }
}
