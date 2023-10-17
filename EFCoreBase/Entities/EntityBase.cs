using EFCoreBase.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EFCoreBase.Entities;

public abstract class EntityBase : IEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
