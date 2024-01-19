using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace EFCore.IncludeByExpression
{
    public interface IIncludable<TEntity>
        where TEntity : class
    {
        // Container for extension methods
    }

    public static class IncludableExtensions
    {
        public static IThenIncludable<TEntity, TProperty> Include<TEntity, TProperty>(
            this IIncludable<TEntity> source,
            in Expression<Func<TEntity, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var context = Unsafe.As<IContext<TEntity>>(source);
            context.Query = context.Query.Include(navigationPropertyPath);
            return Unsafe.As<IThenIncludable<TEntity, TProperty>>(context);
        }
    }
}
