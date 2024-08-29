using EFCore.IncludeByExpression.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EFCore.IncludeByExpression
{
    public static class ThenIncludableExtensions
    {
        /// <summary>
        ///     Specifies related entities to include in the query results.
        ///     The navigation property to be included is specified starting with the
        ///     type of entity being queried (<typeparamref name="TEntity" />).
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The context providing source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A IThenIncludable interface.</returns>
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
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = Unsafe
                .As<IIncludableQueryable<TEntity, TPreviousProperty>>(context.Query)
                .ThenInclude(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(context);
        }

        /// <summary>
        ///     Specifies related entities to include in the query results.
        ///     The navigation property to be included is specified starting with the
        ///     type of entity being queried (<typeparamref name="TEntity" />).
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The context providing source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the navigation property to be included (<c>t => t.Property1</c>).
        /// </param>
        /// <returns>A IThenIncludable interface.</returns>
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
