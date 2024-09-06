using System.Linq;

namespace EFCore.IncludeByExpression.Abstractions
{
    internal interface IContext<TEntity>
    {
        IQueryable<TEntity> Query { get; set; }
    }
}
