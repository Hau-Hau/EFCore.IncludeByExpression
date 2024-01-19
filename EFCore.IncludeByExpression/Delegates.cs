namespace EFCore.IncludeByExpression
{
    public delegate void NavigationPropertyPath<TEntity>(IIncludable<TEntity> query)
        where TEntity : class;
}
