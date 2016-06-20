using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataModel
{
    public class Account
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Salt { get; set; }

        public string PasswordHash { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public Permissions Permission { get; set; }

        public enum Permissions
        {
            Default,
            Admin
        }
    }
}
