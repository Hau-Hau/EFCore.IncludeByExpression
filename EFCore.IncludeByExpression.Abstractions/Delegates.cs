using System.Linq.Expressions;

namespace EFCore.IncludeByExpression.Abstractions
{
    /// <summary>
    ///     A lambda expression representing the chain of navigation properties to be included.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
    /// <param name="query">
    ///     An instance of <see cref="IIncludable{TEntity}" /> representing the query
    ///     to which the navigation properties should be applied.
    /// </param>
    public delegate void NavigationPropertyPath<TEntity>(IIncludable<TEntity> query)
        where TEntity : class;

    internal delegate IThenIncludable<TEntity, TProperty> IncludeDelegate<TEntity, TProperty>(
        IIncludable<TEntity> source,
        Expression<Func<TEntity, TProperty>> navigationPropertyPath
    )
        where TEntity : class;

    internal delegate IThenIncludable<TEntity, TProperty> ThenIncludeDelegate<TEntity, TPreviousProperty, TProperty>(
        IThenIncludable<TEntity, TPreviousProperty?> source,
        in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
    )
        where TEntity : class;

    internal delegate IThenIncludable<TEntity, TProperty> ThenIncludeEnumerableDelegate<
        TEntity,
        TPreviousProperty,
        TProperty
    >(
        IThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source,
        in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
    )
        where TEntity : class;
}
