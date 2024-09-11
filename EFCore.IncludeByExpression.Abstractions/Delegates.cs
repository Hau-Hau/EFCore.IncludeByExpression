using System;
using System.Collections.Generic;
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

    internal delegate void IncludeDelegate(
        Type entityType,
        Type propertyType,
        IContext source,
        LambdaExpression navigationPropertyPath
    );

    internal delegate void ThenIncludeReferenceDelegate(
        Type entityType,
        Type previousPropertyType,
        Type propertyType,
        IContext context,
        LambdaExpression navigationPropertyPath
    );

    internal delegate void ThenIncludeEnumerableDelegate(
        Type entityType,
        Type previousPropertyType,
        Type propertyType,
        IContext context,
        LambdaExpression navigationPropertyPath
    );
}
