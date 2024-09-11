using System.Linq;
using EFCore.IncludeByExpression.Abstractions;

namespace EFCore.IncludeByExpression
{
    internal sealed class Context<TEntity, TProperty>
        : IContext,
            IIncludable<TEntity>,
            IThenIncludable<TEntity, TProperty>
        where TEntity : class
    {
        public Context(IQueryable<TEntity> query)
        {
            Query = query;
        }

        public IQueryable Query { get; set; }
    }
}
