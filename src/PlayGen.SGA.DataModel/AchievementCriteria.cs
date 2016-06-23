using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataModel
{
    public class AchievementCriteria
    {
        public string Key { get; set; }

        public DataType DataType { get; set; }

        public ComparisonType ComparisonType { get; set; }

        public string Value { get; set; }
    }
}
