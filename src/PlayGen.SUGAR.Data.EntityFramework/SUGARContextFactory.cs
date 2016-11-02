using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework
{
    public class SUGARContextFactory
    {
        private readonly string _connectionString;

        public SUGARContextFactory(string connectionString = null)
        {
            _connectionString = connectionString;
        }

        public SUGARContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SUGARContext>();
            optionsBuilder.UseMySQL(_connectionString);

            var context = new SUGARContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
