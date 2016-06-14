using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
namespace PlayGen.SGA.DataController
{
    public abstract class DbController
    {
        protected readonly string _nameOrConnectionString;

        protected DbController(string nameOrConnectionString)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }
        
        protected void SetLog(DbContext context)
        {
            context.Database.Log = Console.WriteLine;
        }
    }
}
