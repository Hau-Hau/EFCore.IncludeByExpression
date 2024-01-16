using EFCore.NavigationPropertyPathSchema.Abstractions;
using EFCore.NavigationPropertyPathSchema.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EFCore.NavigationPropertyPathSchema
{
    public static class NavigationPropertyPathSchema
    {
        public static ISchemaExecutable<T> For<T>(DbSet<T> dbSet)
            where T : class
        {
            return new NavigationPropertyPathSchema<T>(dbSet);
        }
    }

    internal struct NavigationPropertyPathSchema<TEntity>
        : ISchemaQueryable<TEntity>,
            ISchemaExecutable<TEntity>,
            IQueryIncludable<TEntity>
        where TEntity : class
    {
        internal NavigationPropertyPathSchema(DbSet<TEntity> query)
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
