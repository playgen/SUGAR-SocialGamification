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
    /// 
    /// See for ref: http://www.reddnet.net/entity-framework-json-column/
    /// </summary>
    
    // TODO make sure this works correctly, didn't have time to test. sorry :(
    public class CompletionCriteriaCollection : Collection<CompletionCriteria>
    {
        public void Add(ICollection<CompletionCriteria> completionCriterias)
        {
            foreach (var completionCriteria in completionCriterias)
            {
                this.Add(completionCriteria);
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

                var jsonData = JsonConvert.DeserializeObject<List<CompletionCriteria>>(value);
                this.Items.Clear();
                this.Add(jsonData);
            }
        }
    }
}
