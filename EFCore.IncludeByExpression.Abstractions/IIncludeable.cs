namespace EFCore.IncludeByExpression.Abstractions
{
    /// <summary>
    ///     Represents an interface that serves as a marker for queryable types
    ///     where navigation properties can be explicitly included.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
    public interface IIncludable<TEntity>
        where TEntity : class
    {
        // Serves as a marker for queryable types that support the inclusion of navigation properties.
    }
}
