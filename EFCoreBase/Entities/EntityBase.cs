using EFCoreBase.Interfaces;

namespace EFCoreBase.Entities;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
