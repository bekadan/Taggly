using Microsoft.EntityFrameworkCore;

namespace Taggly.SharedKernel.Abstractions.Data;

/// <summary>
/// Represents the application database context interface.
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// Gets the database set for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <returns>The database set for the specified entity type.</returns>
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class, IEntity;

    /// <summary>
    /// Gets the entity with the specified identifier.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="id">The entity identifier.</param>
    /// <returns>The <typeparamref name="TEntity"/> with the specified identifier if it exists, otherwise null.</returns>
    Task<TEntity?> GetBydIdAsync<TEntity>(Guid id)
        where TEntity : IEntity;

    /// <summary>
    /// Inserts the specified entity into the database context.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be inserted into the database context.</param>
    void Insert<TEntity>(TEntity entity)
        where TEntity : IEntity;

    /// <summary>
    /// Removes the specified entity from the database context.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="entity">The entity to be removed from the database context.</param>
    void Remove<TEntity>(TEntity entity)
        where TEntity : IEntity;
}
