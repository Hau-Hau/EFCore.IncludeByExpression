using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using EFCore.IncludeByExpression.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.IncludeByExpression
{
    internal static class IncludableService
    {
        public static IThenIncludable<TEntity, TNextProperty> Include<TEntity, TNextProperty>(
            IIncludable<TEntity> source,
            Expression<Func<TEntity, TNextProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = context.Query.Include(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TNextProperty>>(context);
        }

        public static IThenIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, TPreviousProperty?> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = Unsafe
                .As<IIncludableQueryable<TEntity, TPreviousProperty>>(context.Query)
                .ThenInclude(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(context);
        }

        public static IThenIncludable<TEntity, TProperty> ThenIncludeEnumerable<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
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
