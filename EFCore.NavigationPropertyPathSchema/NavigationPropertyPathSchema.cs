using EFCore.NavigationPropertyPathSchema.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.NavigationPropertyPathSchema
{
    public static class NavigationPropertyPathSchema
    {
        public static ISchemaExecutable<T> For<T>(DbSet<T> dbSet)
            where T : class
        {
            return new NavigationPropertyPathSchemaImpl<T>(dbSet);
        }
    }
}
