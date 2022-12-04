using System.Linq;

namespace EFCore.NavigationPropertyPathSchema.Abstractions
{
    internal interface ISchemaQueryable<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query { get; set; }
    }
}
