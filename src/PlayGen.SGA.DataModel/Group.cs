using System;
using System.Collections.Generic;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class Group : IRecord
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
