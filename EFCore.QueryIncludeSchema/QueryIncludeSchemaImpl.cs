using Microsoft.EntityFrameworkCore;
using EFCore.QueryIncludeSchema.Data;
using EFCore.QueryIncludeSchema.Interfaces;

namespace EFCore.QueryIncludeSchema
{
    internal record struct QueryIncludeSchemaImpl<TEntity> : ISchemaQueryable<TEntity>, ISchemaExecutable<TEntity>, IQueryIncludable<TEntity>
        where TEntity : class
    {
        public QueryIncludeSchemaImpl(DbSet<TEntity> query)
        {
            Query = query.AsQueryable();
        }

        public IQueryable<TEntity> Query { get; set; }

        public IQueryable<TEntity> Execute(IncludePropertyPath<TEntity>? includePropertyPath = null)
        {
            ISchemaQueryable<TEntity> that = this;
            includePropertyPath?.Invoke(new SchemaContainer<TEntity>(ref that));
            return that.Query;
        }
    }
}