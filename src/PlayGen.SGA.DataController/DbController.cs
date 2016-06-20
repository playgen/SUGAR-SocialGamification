using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;

namespace PlayGen.SGA.DataController
{
    public abstract class DbController
    {
        protected readonly string NameOrConnectionString;
        protected readonly DbExceptionHandler _dbExceptionHandler = new DbExceptionHandler();

        protected DbController(string nameOrConnectionString)
        {
            NameOrConnectionString = nameOrConnectionString;
        }
        
        protected void SetLog(DbContext context)
        {
            context.Database.Log = Console.WriteLine;
        }

        protected void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                _dbExceptionHandler.Handle(exception);
            }
        }
    }
}
