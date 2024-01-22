using System.Linq;

namespace EFCore.IncludeByExpression
{
    internal interface IContext<TEntity>
    {
        IQueryable<TEntity> Query { get; set; }
    }
}
