// Domain/Abstractions/Entity.cs
namespace Booksworm.Domain.Abstractions;

/// <summary>
/// Represents an entity in the system with a unique identifier.
/// </summary>
/// <typeparam name="TId">The type of the entity's ID. Typically <see cref="Guid"/>.</typeparam>
public abstract class Entity<TId> where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Gets the ID of the entity.
    /// </summary>
    public TId Id { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    protected Entity(TId id) => Id = id;

    /// <summary>
    /// Protected constructor for Entity Framework Core (for lazy loading and materialization).
    /// </summary>
    protected Entity() => Id = default!;
}