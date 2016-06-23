using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlayGen.SGA.DataModel
{
    /// <summary>
    /// Class that gets stored as a string in a single column in the database:
    /// See for ref: http://www.reddnet.net/entity-framework-json-column/
    /// </summary>
    
    public class AchievementCriteriaCollection : Collection<AchievementCriteria>
    {
        public void Add(ICollection<AchievementCriteria> completionCriterias)
        {
            foreach (var completionCriteria in completionCriterias)
            {
                Add(completionCriteria);
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

                var jsonData = JsonConvert.DeserializeObject<List<AchievementCriteria>>(value);
                Items.Clear();
                Add(jsonData);
            }
        }
    }
}
