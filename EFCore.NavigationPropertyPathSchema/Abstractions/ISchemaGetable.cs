namespace EFCore.NavigationPropertyPathSchema.Abstractions
{
    internal interface ISchemaGetable<TEntity>
        where TEntity : class
    {
        ISchemaQueryable<TEntity> Schema { get; set; }
    }
}
