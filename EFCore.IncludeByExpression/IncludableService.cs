using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EFCore.IncludeByExpression.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.IncludeByExpression
{
    internal static class IncludableService
    {
        private static readonly MethodInfo IncludeMethodInfo = typeof(EntityFrameworkQueryableExtensions)
            .GetTypeInfo()
            .GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.Include))
            .First(mi =>
                mi.GetGenericArguments().Length == 2
                && mi.GetParameters()
                    .Any(pi => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string))
            );

        private static readonly MethodInfo ThenIncludeAfterReferenceMethodInfo =
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .First(mi =>
                    mi.GetGenericArguments().Length == 3
                    && mi.GetParameters()[0].ParameterType.GenericTypeArguments[1].IsGenericParameter
                );

        private static readonly MethodInfo ThenIncludeAfterEnumerableMethodInfo =
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods(nameof(EntityFrameworkQueryableExtensions.ThenInclude))
                .Where(mi => mi.GetGenericArguments().Length == 3)
                .First(mi =>
                {
                    var typeInfo = mi.GetParameters()[0].ParameterType.GenericTypeArguments[1];
                    return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                });

        public static void Include(
            Type entityType,
            Type propertyType,
            IContext context,
            LambdaExpression navigationPropertyPath
        )
        {
            context.Query = (IQueryable)
                IncludeMethodInfo
                    .MakeGenericMethod(entityType, propertyType)
                    .Invoke(null, new object[] { context.Query, navigationPropertyPath })!;
        }

        public static void ThenIncludeReference(
            Type entityType,
            Type previousPropertyType,
            Type propertyType,
            IContext context,
            LambdaExpression navigationPropertyPath
        )
        {
            context.Query = (IQueryable)
                ThenIncludeAfterReferenceMethodInfo
                    .MakeGenericMethod(entityType, previousPropertyType, propertyType)
                    .Invoke(null, new object[] { context.Query, navigationPropertyPath })!;
        }

        public static void ThenIncludeEnumerable(
            Type entityType,
            Type previousPropertyType,
            Type propertyType,
            IContext context,
            LambdaExpression navigationPropertyPath
        )
        {
            context.Query = (IQueryable)
                ThenIncludeAfterEnumerableMethodInfo
                    .MakeGenericMethod(entityType, previousPropertyType, propertyType)
                    .Invoke(null, new object[] { context.Query, navigationPropertyPath })!;
        }
    }
}
