using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using EFCore.QueryIncludeSchema.Data;
using System.Linq.Expressions;

namespace EFCore.QueryIncludeSchema.Interfaces
{
    public interface IQueryThenIncludable<TEntity, out TProperty> : IQueryIncludable<TEntity>
        where TEntity : class
    {
    }

    public static class IQueryThenIncludableExtensions
    {
        public static IQueryThenIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IQueryThenIncludable<TEntity, TPreviousProperty?> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            var schema = ((ISchemaGetable<TEntity>)source).Schema;
            schema.Query = ((IIncludableQueryable<TEntity, TPreviousProperty>)schema.Query).ThenInclude(navigationPropertyPath);
            return new ThenIncludeSchemaContainer<TEntity, TProperty>(ref schema);
        }

        public static IQueryThenIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IQueryThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {

            var schema = ((ISchemaGetable<TEntity>)source).Schema;
            schema.Query = ((IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>)schema.Query).ThenInclude(navigationPropertyPath);
            return new ThenIncludeSchemaContainer<TEntity, TProperty>(ref schema);
        }
    }
}
