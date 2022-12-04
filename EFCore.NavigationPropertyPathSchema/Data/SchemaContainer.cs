using EFCore.NavigationPropertyPathSchema.Abstractions;

namespace EFCore.NavigationPropertyPathSchema.Data
{
    internal struct SchemaContainer<TEntity> : IQueryIncludable<TEntity>, ISchemaGetable<TEntity>
        where TEntity : class
    {
        public ISchemaQueryable<TEntity> Schema { get; set; }

        public SchemaContainer(ref ISchemaQueryable<TEntity> schema)
        {
            Schema = schema;
        }
    }

    internal struct ThenIncludeSchemaContainer<TEntity, TProperty> : IQueryThenIncludable<TEntity, TProperty>, ISchemaGetable<TEntity>
        where TEntity : class
    {
        public ISchemaQueryable<TEntity> Schema { get; set; }

        public ThenIncludeSchemaContainer(ref ISchemaQueryable<TEntity> schema)
        {
            Schema = schema;
        }
    }
}
