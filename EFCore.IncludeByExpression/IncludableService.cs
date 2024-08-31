using EFCore.IncludeByExpression.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace EFCore.IncludeByExpression
{
    internal class IncludableService<TEntity> : IIncludableService<TEntity>
        where TEntity : class
    {
        public IThenIncludable<TEntity, TNextProperty> Include<TNextProperty>(
            IIncludable<TEntity> source,
            in Expression<Func<TEntity, TNextProperty>> navigationPropertyPath)
        {
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = context.Query.Include(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TNextProperty>>(context);
        }

        public IThenIncludable<TEntity, TProperty> ThenInclude<TPreviousProperty, TProperty>(IThenIncludable<TEntity, TPreviousProperty?> source, in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        {
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = Unsafe
                .As<IIncludableQueryable<TEntity, TPreviousProperty>>(context.Query)
                .ThenInclude(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(context);
        }

        public IThenIncludable<TEntity, TProperty> ThenIncludeEnumerable<TPreviousProperty, TProperty>(IThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source, in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        {
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = Unsafe
                .As<IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>>(context.Query)
                .ThenInclude(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(context);
        }
    }
}
