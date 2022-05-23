using System.Linq;

namespace EFCore.QueryIncludeSchema.Interfaces
{
    public interface ISchemaExecutable<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Execute(IncludePropertyPath<TEntity>? includePropertyPath = null);
    }
}
