namespace EFCore.QueryIncludeSchema.Interfaces
{
    internal interface ISchemaQueryable<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query { get; set; }
    }
}
