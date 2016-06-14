using System;
using System.Collections.Generic;
using PlayGen.SGA.DataModel.Interfaces;

namespace PlayGen.SGA.DataModel
{
    public class Game : IRecord
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<GroupAchievement> GroupAchievements { get; set; }

        public virtual List<GroupData> GroupDatas { get; set; }

        public virtual List<UserAchievement> UserAchievements { get; set; }

        public virtual List<UserData> UserDatas { get; set; }

        public Game(string name)
        {
            Name = name;
        }
    }
}
