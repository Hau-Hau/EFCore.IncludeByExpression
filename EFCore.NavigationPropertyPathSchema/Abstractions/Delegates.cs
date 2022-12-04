namespace EFCore.NavigationPropertyPathSchema.Abstractions
{
    public delegate void IncludePropertyPath<TEntity>(IQueryIncludable<TEntity> query)
        where TEntity : class;
}
