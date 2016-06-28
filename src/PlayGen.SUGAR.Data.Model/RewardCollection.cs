using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Data.Model
{
	/// <summary>
	/// Class that gets stored as a string in a single column in the database:
	/// See for ref: http://www.reddnet.net/entity-framework-json-column/
	/// </summary>

	public class RewardCollection : Collection<Reward>
	{
		public void Add(ICollection<Reward> rewards)
		{
			foreach (var reward in rewards)
			{
				Add(reward);
			}
		}

		[JsonIgnore]
		public string Serialised
		{
			get { return JsonConvert.SerializeObject(this); }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}

				var jsonData = JsonConvert.DeserializeObject<List<Reward>>(value);
				Items.Clear();
				Add(jsonData);
			}
		}
	}
}
