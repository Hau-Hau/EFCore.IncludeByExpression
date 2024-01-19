using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.IncludeByExpression
{
    public interface IThenIncludable<TEntity, out TProperty> : IIncludable<TEntity>
        where TEntity : class
    {
        // Container for extension methods
    }

    public static class ThenIncludableExtensions
    {
        public static IThenIncludable<TEntity, TProperty> ThenInclude<
            TEntity,
            TPreviousProperty,
            TProperty
        >(
            this IThenIncludable<TEntity, TPreviousProperty?> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var context = (IContext<TEntity>)source;
            context.Query = Unsafe
                .As<IIncludableQueryable<TEntity, TPreviousProperty>>(context.Query)
                .ThenInclude(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(context);
        }

        public static IThenIncludable<TEntity, TProperty> ThenInclude<
            TEntity,
            TPreviousProperty,
            TProperty
        >(
            this IThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = Unsafe
                .As<IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>>(context.Query)
                .ThenInclude(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(context);
        }
    }
}
