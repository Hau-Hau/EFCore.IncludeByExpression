using System.Linq;

namespace EFCore.IncludeByExpression
{
    internal interface IContext<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query { get; set; }
    }
}
