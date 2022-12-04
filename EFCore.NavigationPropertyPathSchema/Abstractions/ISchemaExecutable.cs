using System.Linq;

namespace EFCore.NavigationPropertyPathSchema.Abstractions
{
    public interface ISchemaExecutable<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Execute(IncludePropertyPath<TEntity>? includePropertyPath = null);
    }
}
