using EFCore.NavigationPropertyPathSchema.Abstractions;
using EFCore.NavigationPropertyPathSchema.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EFCore.NavigationPropertyPathSchema
{
    internal struct NavigationPropertyPathSchemaImpl<TEntity> : ISchemaQueryable<TEntity>, ISchemaExecutable<TEntity>, IQueryIncludable<TEntity>
        where TEntity : class
    {
        public NavigationPropertyPathSchemaImpl(DbSet<TEntity> query)
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