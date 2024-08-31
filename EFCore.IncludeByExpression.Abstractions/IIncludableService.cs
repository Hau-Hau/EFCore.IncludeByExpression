using System.Collections.Generic;
using System.Linq.Expressions;

namespace EFCore.IncludeByExpression.Abstractions
{
    internal interface IIncludableService<TEntity> where TEntity : class
    {
        IThenIncludable<TEntity, TNextProperty> Include<TNextProperty>(
            IIncludable<TEntity> source,
            in Expression<Func<TEntity, TNextProperty>> navigationPropertyPath
        );

        IThenIncludable<TEntity, TProperty> ThenInclude<
            TPreviousProperty,
            TProperty
        >(
            IThenIncludable<TEntity, TPreviousProperty?> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        );

        IThenIncludable<TEntity, TProperty> ThenIncludeEnumerable<
            TPreviousProperty,
            TProperty
        >(
            IThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        );
    }
}
