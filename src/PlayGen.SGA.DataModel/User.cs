using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class User : IRecord
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public User(string name)
        {
            Name = name;
        }
    }
}
