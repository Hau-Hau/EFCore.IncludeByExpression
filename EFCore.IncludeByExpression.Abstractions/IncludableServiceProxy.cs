using System.Collections.Concurrent;
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
        private static readonly ConcurrentDictionary<(Type, Type), Delegate> IncludeDelegateCache = new();
        private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ThenIncludeDelegateCache = new();
        private static readonly ConcurrentDictionary<(Type, Type, Type), Delegate> ThenIncludeEnumerableDelegateCache =
            new();

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
                serviceType.GetMethod(name: "ThenInclude", bindingAttr: BindingFlags.Public | BindingFlags.Static)
                ?? throw new InvalidOperationException("ThenInclude method not found.");
            ThenIncludeEnumerableMethod =
                serviceType.GetMethod("ThenIncludeEnumerable", BindingFlags.Public | BindingFlags.Static)
                ?? throw new InvalidOperationException("ThenIncludeEnumerable method not found.");
        }

        public static IThenIncludable<TEntity, TProperty> Include<TEntity, TProperty>(
            IIncludable<TEntity> source,
            in Expression<Func<TEntity, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var includeDelegate = IncludeDelegateCache.GetOrAdd(
                (typeof(TEntity), typeof(TProperty)),
                _ =>
                {
                    var genericMethod = IncludeMethod.MakeGenericMethod(typeof(TEntity), typeof(TProperty));
                    var sourceParam = Expression.Parameter(typeof(IIncludable<TEntity>), "source");
                    var pathParam = Expression.Parameter(
                        typeof(Expression<Func<TEntity, TProperty>>),
                        "navigationPropertyPath"
                    );
                    var callExpression = Expression.Call(genericMethod, sourceParam, pathParam);
                    var lambda = Expression.Lambda(callExpression, sourceParam, pathParam);
                    return lambda.Compile();
                }
            );
            return Unsafe
                .As<IncludeDelegate<TEntity, TProperty>>(includeDelegate)
                .Invoke(source, navigationPropertyPath);
        }

        public static IThenIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, TPreviousProperty?> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var thenIncludeDelegate = ThenIncludeDelegateCache.GetOrAdd(
                (typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)),
                _ =>
                {
                    var genericMethod = ThenIncludeMethod.MakeGenericMethod(
                        typeof(TEntity),
                        typeof(TPreviousProperty),
                        typeof(TProperty)
                    );
                    var sourceParam = Expression.Parameter(
                        typeof(IThenIncludable<TEntity, TPreviousProperty>),
                        "source"
                    );
                    var pathParam = Expression.Parameter(
                        typeof(Expression<Func<TPreviousProperty, TProperty>>),
                        "navigationPropertyPath"
                    );
                    var callExpression = Expression.Call(genericMethod, sourceParam, pathParam);
                    var lambda = Expression.Lambda(callExpression, sourceParam, pathParam);
                    return lambda.Compile();
                }
            );
            return Unsafe
                .As<ThenIncludeDelegate<TEntity, TPreviousProperty, TProperty>>(thenIncludeDelegate)
                .Invoke(source, navigationPropertyPath);
        }

        public static IThenIncludable<TEntity, TProperty> ThenIncludeEnumerable<TEntity, TPreviousProperty, TProperty>(
            IThenIncludable<TEntity, IEnumerable<TPreviousProperty>?> source,
            in Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath
        )
            where TEntity : class
        {
            var thenIncludeEnumerableDelegate = ThenIncludeEnumerableDelegateCache.GetOrAdd(
                (typeof(TEntity), typeof(TPreviousProperty), typeof(TProperty)),
                _ =>
                {
                    var genericMethod = ThenIncludeEnumerableMethod.MakeGenericMethod(
                        typeof(TEntity),
                        typeof(TPreviousProperty),
                        typeof(TProperty)
                    );
                    var sourceParam = Expression.Parameter(
                        typeof(IThenIncludable<TEntity, IEnumerable<TPreviousProperty>>),
                        "source"
                    );
                    var pathParam = Expression.Parameter(
                        typeof(Expression<Func<TPreviousProperty, TProperty>>),
                        "navigationPropertyPath"
                    );
                    var callExpression = Expression.Call(genericMethod, sourceParam, pathParam);
                    var lambda = Expression.Lambda(callExpression, sourceParam, pathParam);
                    return lambda.Compile();
                }
            );
            return Unsafe
                .As<ThenIncludeEnumerableDelegate<TEntity, TPreviousProperty, TProperty>>(thenIncludeEnumerableDelegate)
                .Invoke(source, navigationPropertyPath);
        }
    }
}
