using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace EFCore.IncludeByExpression.Abstractions
{
    internal static class IncludableServiceProxy
    {
        private static readonly MethodInfo IncludeMethod;
        private static readonly MethodInfo ThenIncludeReferenceMethod;
        private static readonly MethodInfo ThenIncludeEnumerableMethod;
        private static IncludeDelegate IncludeDelegate;
        private static ThenIncludeReferenceDelegate ThenIncludeReferenceDelegate;
        private static ThenIncludeEnumerableDelegate ThenIncludeEnumerableDelegate;

        static IncludableServiceProxy()
        {
            var assembly = Assembly.Load("EFCore.IncludeByExpression");
            var serviceType =
                assembly.GetType("EFCore.IncludeByExpression.IncludableService")
                ?? throw new InvalidOperationException("IncludableService type not found.");
            IncludeMethod =
                serviceType.GetMethod("Include", BindingFlags.Public | BindingFlags.Static)
                ?? throw new InvalidOperationException("Include method not found.");
            ThenIncludeReferenceMethod =
                serviceType.GetMethod(
                    name: "ThenIncludeReference",
                    bindingAttr: BindingFlags.Public | BindingFlags.Static
                ) ?? throw new InvalidOperationException("ThenInclude method not found.");
            ThenIncludeEnumerableMethod =
                serviceType.GetMethod("ThenIncludeEnumerable", BindingFlags.Public | BindingFlags.Static)
                ?? throw new InvalidOperationException("ThenIncludeEnumerable method not found.");
        }

        public static void Include<TEntity, TProperty>(
            IIncludable<TEntity> source,
            in System.Linq.Expressions.Expression<Func<TEntity, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            if (IncludeDelegate == null)
            {
                var entityTypeParam = Expression.Parameter(typeof(Type), "entityType");
                var propertyTypeParam = Expression.Parameter(typeof(Type), "propertyType");
                var contextParam = Expression.Parameter(typeof(IContext), "context");
                var pathParam = Expression.Parameter(typeof(LambdaExpression), "navigationPropertyPath");
                var callExpression = Expression.Call(
                    IncludeMethod,
                    entityTypeParam,
                    propertyTypeParam,
                    contextParam,
                    pathParam
                );
                var lambda = Expression.Lambda<IncludeDelegate>(
                    callExpression,
                    new List<ParameterExpression>() { entityTypeParam, propertyTypeParam, contextParam, pathParam }
                );
                IncludeDelegate = lambda.Compile();
            }
            IncludeDelegate.Invoke(typeof(TEntity), typeof(TProperty), (IContext)source, navigationPropertyPath);
        }

        public static void ThenIncludeReference<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, TPreviousProperty> source,
            in System.Linq.Expressions.Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            if (ThenIncludeReferenceDelegate == null)
            {
                var entityTypeParam = Expression.Parameter(typeof(Type), "entityType");
                var previousPropertyTypeParam = Expression.Parameter(typeof(Type), "previousPropertyType");
                var propertyTypeParam = Expression.Parameter(typeof(Type), "propertyType");
                var contextParam = Expression.Parameter(typeof(IContext), "context");
                var pathParam = Expression.Parameter(typeof(LambdaExpression), "navigationPropertyPath");
                var callExpression = Expression.Call(
                    ThenIncludeReferenceMethod,
                    entityTypeParam,
                    previousPropertyTypeParam,
                    propertyTypeParam,
                    contextParam,
                    pathParam
                );
                var lambda = Expression.Lambda<ThenIncludeReferenceDelegate>(
                    callExpression,
                    new List<ParameterExpression>()
                    {
                        entityTypeParam,
                        previousPropertyTypeParam,
                        propertyTypeParam,
                        contextParam,
                        pathParam,
                    }
                );
                ThenIncludeReferenceDelegate = lambda.Compile();
            }
            ThenIncludeReferenceDelegate.Invoke(
                typeof(TEntity),
                typeof(TPreviousProperty),
                typeof(TProperty),
                (IContext)source,
                navigationPropertyPath
            );
        }

        public static void ThenIncludeEnumerable<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, IEnumerable<TPreviousProperty>> source,
            in System.Linq.Expressions.Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            if (ThenIncludeEnumerableDelegate == null)
            {
                var entityTypeParam = Expression.Parameter(typeof(Type), "entityType");
                var previousPropertyTypeParam = Expression.Parameter(typeof(Type), "previousPropertyType");
                var propertyTypeParam = Expression.Parameter(typeof(Type), "propertyType");
                var contextParam = Expression.Parameter(typeof(IContext), "context");
                var pathParam = Expression.Parameter(typeof(LambdaExpression), "navigationPropertyPath");
                var callExpression = Expression.Call(
                    ThenIncludeEnumerableMethod,
                    entityTypeParam,
                    previousPropertyTypeParam,
                    propertyTypeParam,
                    contextParam,
                    pathParam
                );
                var lambda = Expression.Lambda<ThenIncludeEnumerableDelegate>(
                    callExpression,
                    new List<ParameterExpression>()
                    {
                        entityTypeParam,
                        previousPropertyTypeParam,
                        propertyTypeParam,
                        contextParam,
                        pathParam,
                    }
                );
                ThenIncludeEnumerableDelegate = lambda.Compile();
            }
            ThenIncludeEnumerableDelegate.Invoke(
                typeof(TEntity),
                typeof(TPreviousProperty),
                typeof(TProperty),
                (IContext)source,
                navigationPropertyPath
            );
        }
    }
}
