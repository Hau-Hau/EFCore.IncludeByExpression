using System.Linq;
using EFCore.IncludeByExpression.Abstractions;

namespace EFCore.IncludeByExpression
{
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Specifies related entities to include in the query results. The navigation property to be included is
        ///     specified starting with the type of entity being queried (<typeparamref name="TEntity" />).
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
        /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
        /// <param name="source">The context providing source query.</param>
        /// <param name="navigationPropertyPath">
        ///     A lambda expression representing the chain of navigation properties to be included.
        /// </param>
        /// <returns>A new query with the related data included.</returns>
        public static IQueryable<TEntity> IncludeByExpression<TEntity>(
            this IQueryable<TEntity> source,
            in NavigationPropertyPath<TEntity>? navigationPropertyPath = null
        )
            where TEntity : class
        {
            if (navigationPropertyPath == null)
            {
                return source;
            }

            var context = new Context<TEntity, TEntity>(source);
            navigationPropertyPath?.Invoke(context);
            return context.Query;
        }
    }
}
