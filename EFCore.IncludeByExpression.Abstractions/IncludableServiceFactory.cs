using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace EFCore.IncludeByExpression.Abstractions
{
    internal static class IncludableServiceFactory
    {
        private static Assembly? ImplementationAssembly = null;
        private static Type? IncludableServiceType = null;
        private static readonly ConcurrentDictionary<Type, Func<object>> ConstructorCache = new();

        public static Func<object> GetActivator(Type type)
        {
            return Expression.Lambda<Func<object>>(Expression.New(type)).Compile();
        }

        public static IIncludableService<TEntity> Create<TEntity>()
            where TEntity : class
        {
            ImplementationAssembly ??= Assembly.Load("EFCore.IncludeByExpression");
            IncludableServiceType ??= ImplementationAssembly
                .GetType($"EFCore.IncludeByExpression.IncludableService`1");

            if (IncludableServiceType == null)
            {
                throw new InvalidOperationException("Implementation of IIncludableService not found.");
            }

            var serviceType = IncludableServiceType?.MakeGenericType(typeof(TEntity));
            if (serviceType == null)
            {
                throw new InvalidOperationException("Cannot make generic IIncludableService.");
            }


            if (!ConstructorCache.TryGetValue(serviceType, out var constructor))
            {
                var ctor = Expression.New(serviceType);
                var lambda = Expression.Lambda<Func<object>>(ctor);
                constructor = lambda.Compile();
                ConstructorCache[serviceType] = constructor;
            }

            return Unsafe.As<IIncludableService<TEntity>>(constructor());
        }
    }
}
