using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EFCore.IncludeByExpression.Abstractions
{
    /// <summary>
    ///     Represents an interface that extends the functionality of <see cref="IIncludable{TEntity}" />,
    ///     allowing the chaining of navigation property inclusions in a query for a given entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
    /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
    public interface IThenIncludable<TEntity, out TProperty> : IIncludable<TEntity>
        where TEntity : class
    {
        // Serves as a marker for queryable types that support the chaining of navigation property inclusions.
    }

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
        public static IThenIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IThenIncludable<TEntity, TPreviousProperty> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            IncludableServiceProxy.ThenIncludeReference(source, navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(source);
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
        public static IThenIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IThenIncludable<TEntity, IEnumerable<TPreviousProperty>> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            IncludableServiceProxy.ThenIncludeEnumerable(source, navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(source);
        }
    }
}
