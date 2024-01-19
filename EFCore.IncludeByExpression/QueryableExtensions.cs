using System.Linq;

namespace EFCore.IncludeByExpression
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> IncludeByExpression<TEntity>(
            this IQueryable<TEntity> source,
            in NavigationPropertyPath<TEntity>? navigationPropertyPath = null
        )
            where TEntity : class
        {
            if (navigationPropertyPath == null)
            {
                return source;
            }

            var context = new Context<TEntity, TEntity>(source);
            navigationPropertyPath?.Invoke(context);
            return context.Query;
        }
    }
}
