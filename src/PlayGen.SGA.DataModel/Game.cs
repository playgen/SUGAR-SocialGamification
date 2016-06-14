using System;
using System.Collections.Generic;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class Game : IRecord
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Game(string name)
        {
            Name = name;
        }
    }
}
