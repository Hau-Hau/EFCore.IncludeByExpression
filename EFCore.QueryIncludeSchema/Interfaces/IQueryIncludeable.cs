using Microsoft.EntityFrameworkCore;
using EFCore.QueryIncludeSchema.Data;
using System.Linq.Expressions;

namespace EFCore.QueryIncludeSchema.Interfaces
{
    public interface IQueryIncludable<TEntity>
        where TEntity : class
    {
    }

    public static class IQueryIncludableExtensions
    {
        public static IQueryThenIncludable<TEntity, TProperty> Include<TEntity, TProperty>(this IQueryIncludable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            var schema = ((ISchemaGetable<TEntity>)source).Schema;
            schema.Query = schema.Query.Include(navigationPropertyPath);
            return new ThenIncludeSchemaContainer<TEntity, TProperty>(ref schema);
        }
    }
}
