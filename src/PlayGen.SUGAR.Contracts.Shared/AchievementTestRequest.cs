using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PlayGen.SUGAR.Contracts.Shared
{
	public class AchievementTestRequest
	{
		[Required]
		public AchievementRequest Achievement { get; set; }

		[Required]
		public List<GameDataRequest> GameDatas { get; set; }
	}
}
