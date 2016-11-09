using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
    /// <summary>
    /// Functionality missing from EF.Core that was available in EF 6
    /// </summary>
    public static class DbSetExtensions
    {
        public static IQueryable<Account> IncludeAll(this DbSet<Account> dbSet)
        {
            return dbSet
                .Include(s => s.User);
        }

        public static IQueryable<Achievement> IncludeAll(this DbSet<Achievement> dbSet)
        {
            return dbSet
                .Include(s => s.CompletionCriterias)
                .Include(s => s.Rewards);
        }

        public static IQueryable<Group> IncludeAll(this DbSet<Group> dbSet)
        {
            return dbSet
                .Include(s => s.UserToGroupRelationships)
                .Include(s => s.UserToGroupRelationshipRequests);
        }

        public static IQueryable<Skill> IncludeAll(this DbSet<Skill> dbSet)
        {
            return dbSet
                .Include(s => s.CompletionCriterias)
                .Include(s => s.Rewards);
        }

        public static IQueryable<User> IncludeAll(this DbSet<User> dbSet)
        {
            return dbSet
                .Include(s => s.UserToGroupRelationships)
                .Include(s => s.UserToGroupRelationshipRequests)
                .Include(s => s.Requestors)
                .Include(s => s.RequestRequestors)
                .Include(s => s.Acceptors)
                .Include(s => s.RequestAcceptors);
        }

        /// <summary>
        /// Currently Find is missing from the Entity framework Core API.
        /// Fix taken from: http://stackoverflow.com/questions/29030472/dbset-doesnt-have-a-find-method-in-ef7
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="set"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public static TEntity Find<TEntity>(this IQueryable<TEntity> set, SUGARContext context, params object[] keyValues) where TEntity : class
        {
            var entityType = context.Model.FindEntityType(typeof(TEntity));
            var key = entityType.FindPrimaryKey();

            var entries = context.ChangeTracker.Entries<TEntity>();

            var i = 0;
            foreach (var property in key.Properties)
            {
                var i1 = i;
                entries = entries.Where(e => e.Property(property.Name).CurrentValue == keyValues[i1]);
                i++;
            }

            var entry = entries.FirstOrDefault();
            if (entry != null)
            {
                // Return the local object if it exists.
                return entry.Entity;
            }

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var query = set.AsQueryable();
            i = 0;
            foreach (var property in key.Properties)
            {
                var i1 = i;
                query = query.Where((Expression<Func<TEntity, bool>>)
                 Expression.Lambda(
                     Expression.Equal(
                         Expression.Property(parameter, property.Name),
                         Expression.Constant(keyValues[i1])),
                     parameter));
                i++;
            }

            // Look in the database
            return query.FirstOrDefault();
        }
    }
}