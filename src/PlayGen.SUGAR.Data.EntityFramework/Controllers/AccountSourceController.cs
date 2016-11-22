using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
    public class AccountSourceController : DbController
    {
        public AccountSourceController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public IEnumerable<AccountSource> Get()
        {
            using (var context = ContextFactory.Create())
            {
                var sources = context.AccountSources.ToList();
                return sources;
            }
        }

        public AccountSource Get(int id)
        {
            using (var context = ContextFactory.Create())
            {
                var source = context.AccountSources.Find(context, id);
                return source;
            }
        }

        public AccountSource Get(string token)
        {
            using (var context = ContextFactory.Create())
            {
                var source = context.AccountSources.SingleOrDefault(s => s.Token == token);
                return source;
            }
        }

        public AccountSource Create(AccountSource source)
        {
            using (var context = ContextFactory.Create())
            {
                context.AccountSources.Add(source);
                SaveChanges(context);

                return source;
            }
        }

        public void Update(AccountSource source)
        {
            using (var context = ContextFactory.Create())
            {
                // todo replace with entire block with: (and update unit tests)
                // context.[tablename].Update(entity);
                // context.SaveChanges();

                var existing = context.AccountSources.Find(context, source.Id);

                if (existing != null)
                {
                    context.Entry(existing).State = EntityState.Modified;
                    existing.Description = source.Description;
                    existing.Token = source.Token;
                    existing.RequiresPassword = source.RequiresPassword;
                    SaveChanges(context);
                }
                else
                {
                    throw new MissingRecordException($"The existing AccountSource with ID {source.Id} could not be found.");
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = ContextFactory.Create())
            {
                // todo why are we removing multiple games?
                // todo should we not also be deleting all data associated with this game?
                var source = context.AccountSources
                    .Where(g => id == g.Id);

                context.AccountSources.RemoveRange(source);
                SaveChanges(context);
            }
        }
    }
}
