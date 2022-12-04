using Microsoft.EntityFrameworkCore;
using EFCore.NavigationPropertyPathSchema.Data;
using System.Linq.Expressions;
using System;

namespace EFCore.NavigationPropertyPathSchema.Abstractions
{
    public interface IQueryIncludable<TEntity>
        where TEntity : class
    {
        // Container for extension methods
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
