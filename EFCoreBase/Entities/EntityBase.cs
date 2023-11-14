using EFCoreBase.Interfaces;
using ProcedureMakerServer.Attributes;
using Reinforced.Typings.Attributes;
using System.ComponentModel.DataAnnotations;
namespace EFCoreBase.Entities;



[TsClass]
public abstract class EntityBase : IEntity, IEquatable<IEntity>
{
    [Key]
    [TsGuid]
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


}
