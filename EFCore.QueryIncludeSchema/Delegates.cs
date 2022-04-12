using EFCore.QueryIncludeSchema.Interfaces;

namespace EFCore.QueryIncludeSchema
{
    public delegate void IncludePropertyPath<TEntity>(IQueryIncludable<TEntity> query)
        where TEntity : class;
}
