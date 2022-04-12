namespace EFCore.QueryIncludeSchema.Interfaces
{
    internal interface ISchemaGetable<TEntity>
        where TEntity : class
    {
        ISchemaQueryable<TEntity> Schema { get; set; }
    }
}
