using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EFCore.IncludeByExpression.Abstractions
{
    internal static class IncludableServiceProxy
    {
        private static readonly MethodInfo IncludeMethod;
        private static readonly MethodInfo ThenIncludeMethod;
        private static readonly MethodInfo ThenIncludeEnumerableMethod;

        //private static readonly SoftConcurrentDictionary<(Type, Type), Delegate> IncludeDelegateCache = new();
        //private static readonly SoftConcurrentDictionary<(Type, Type, Type), Delegate> ThenIncludeDelegateCache = new();
        //private static readonly SoftConcurrentDictionary<
        //    (Type, Type, Type),
        //    Delegate
        //> ThenIncludeEnumerableDelegateCache = new();

        static IncludableServiceProxy()
        {
            var assembly = Assembly.Load("EFCore.IncludeByExpression");
            var serviceType =
                assembly.GetType("EFCore.IncludeByExpression.IncludableService")
                ?? throw new InvalidOperationException("IncludableService type not found.");
            IncludeMethod =
                serviceType.GetMethod("Include", BindingFlags.Public | BindingFlags.Static)
                ?? throw new InvalidOperationException("Include method not found.");
            ThenIncludeMethod =
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
                new List<ParameterExpression>()
                {
                    entityTypeParam,
                    propertyTypeParam,
                    contextParam,
                    pathParam,
                }.AsReadOnly()
            );
            var includeDelegate = lambda.Compile();
            includeDelegate.Invoke(
                typeof(TEntity),
                typeof(TProperty),
                Unsafe.As<IContext>(source),
                navigationPropertyPath
            );
        }

        public static void ThenIncludeReference<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, TPreviousProperty> source,
            in System.Linq.Expressions.Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var entityTypeParam = Expression.Parameter(typeof(Type), "entityType");
            var previousPropertyTypeParam = Expression.Parameter(typeof(Type), "previousPropertyType");
            var propertyTypeParam = Expression.Parameter(typeof(Type), "propertyType");
            var contextParam = Expression.Parameter(typeof(IContext), "context");
            var pathParam = Expression.Parameter(typeof(LambdaExpression), "navigationPropertyPath");
            var callExpression = Expression.Call(
                ThenIncludeMethod,
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
                }.AsReadOnly()
            );
            var thenIncludeDelegate = lambda.Compile();
            thenIncludeDelegate.Invoke(
                typeof(TEntity),
                typeof(TPreviousProperty),
                typeof(TProperty),
                Unsafe.As<IContext>(source),
                navigationPropertyPath
            );
        }

        public static void ThenIncludeEnumerable<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, IEnumerable<TPreviousProperty>> source,
            in System.Linq.Expressions.Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
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
                }.AsReadOnly()
            );
            var thenIncludeEnumerableDelegate = lambda.Compile();
            thenIncludeEnumerableDelegate.Invoke(
                typeof(TEntity),
                typeof(TPreviousProperty),
                typeof(TProperty),
                Unsafe.As<IContext>(source),
                navigationPropertyPath
            );
        }
    }
}
