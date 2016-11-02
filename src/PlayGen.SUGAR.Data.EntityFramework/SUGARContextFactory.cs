using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework
{
    public class SUGARContextFactory
    {
        public string ConnectionString { get; set; }

        public SUGARContext Create()
        {
            if (ConnectionString == null)
            {
                return null;
            }

            return Create(ConnectionString);
        }

        public SUGARContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SUGARContext>();
            optionsBuilder.UseMySQL(connectionString);

            var context = new SUGARContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
