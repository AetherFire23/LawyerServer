using EFCoreBase.Interfaces;
namespace EFCoreBase.Entities;

public abstract class EntityBase : IEntity, IEquatable<IEntity>
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public override bool Equals(object? obj)
    {
        return Equals(obj as IEntity);
    }

    public bool Equals(IEntity? other)
    {
        return other is not null &&
               Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    /// <summary>
    /// useful for when you want to generate an id for an entity that is coming from the client
    /// </summary>
    /// <returns></returns>
    public Guid GenerateIdIfNull()
    {
        if (this.Id.Equals(Guid.Empty))
        {
            this.Id = Guid.NewGuid();

        }
        return Id;
    }

    public bool CompareIds(EntityBase other)
    {
        bool matchFound = other.Id == this.Id;
        return matchFound;
    }
}
