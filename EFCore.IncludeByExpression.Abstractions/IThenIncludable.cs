namespace EFCore.IncludeByExpression.Abstractions
{
    /// <summary>
    ///     Represents an interface that extends the functionality of <see cref="IIncludable{TEntity}" />,
    ///     allowing the chaining of navigation property inclusions in a query for a given entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
    /// <typeparam name="TProperty">The type of the related entity to be included.</typeparam>
    public interface IThenIncludable<TEntity, out TProperty> : IIncludable<TEntity>
        where TEntity : class
    {
        // Serves as a marker for queryable types that support the chaining of navigation property inclusions.
    }
}
