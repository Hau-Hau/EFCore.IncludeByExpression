using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using EFCore.NavigationPropertyPathSchema.Data;
using System.Linq.Expressions;
using System;
using System.Collections.Generic;

namespace EFCore.NavigationPropertyPathSchema.Abstractions
{
    public interface IQueryThenIncludable<TEntity, out TProperty> : IQueryIncludable<TEntity>
        where TEntity : class
    {
        // Container for extension methods
    }

    public static class IQueryThenIncludableExtensions
    {
#if NET6_0_OR_GREATER
        public static IQueryThenIncludable<TEntity, TProperty> ThenInclude<
            TEntity,
            TPreviousProperty,
            TProperty
        >(
            this IQueryThenIncludable<TEntity, TPreviousProperty?> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
#else
        public static IQueryThenIncludable<TEntity, TProperty> ThenInclude<
            TEntity,
            TPreviousProperty,
            TProperty
        >(
            this IQueryThenIncludable<TEntity, TPreviousProperty> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
#endif
            where TEntity : class
        {
            var schema = ((ISchemaGetable<TEntity>)source).Schema;
            schema.Query = (
                (IIncludableQueryable<TEntity, TPreviousProperty>)schema.Query
            ).ThenInclude(navigationPropertyPath);
            return new ThenIncludeSchemaContainer<TEntity, TProperty>(ref schema);
        }

#if NET6_0_OR_GREATER
        public static IQueryThenIncludable<TEntity, TProperty> ThenInclude<
            TEntity,
            TPreviousProperty,
            TProperty
        >(
            this IQueryThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
#else
        public static IQueryThenIncludable<TEntity, TProperty> ThenInclude<
            TEntity,
            TPreviousProperty,
            TProperty
        >(
            this IQueryThenIncludable<TEntity, IEnumerable<TPreviousProperty>> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
#endif
            where TEntity : class
        {
            var schema = ((ISchemaGetable<TEntity>)source).Schema;
            schema.Query = (
                (IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>)schema.Query
            ).ThenInclude(navigationPropertyPath);
            return new ThenIncludeSchemaContainer<TEntity, TProperty>(ref schema);
        }
    }
}
