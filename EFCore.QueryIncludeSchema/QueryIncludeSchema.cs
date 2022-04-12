using Microsoft.EntityFrameworkCore;
using EFCore.QueryIncludeSchema.Interfaces;

namespace EFCore.QueryIncludeSchema
{
    public static class QueryIncludeSchema
    {
        public static ISchemaExecutable<T> For<T>(DbSet<T> dbSet)
            where T : class
        {
            return new QueryIncludeSchemaImpl<T>(dbSet);
        }
    }
}
