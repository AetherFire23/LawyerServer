using EFCoreBase.Interfaces;
using ProcedureMakerServer.Attributes;
using Reinforced.Typings.Attributes;
using System.ComponentModel.DataAnnotations;
namespace EFCoreBase.Entities;



[TsClass]
public abstract class EntityBase : IEntity
{
    [Key]
    [TsGuid]
    public Guid Id { get; set; } = Guid.NewGuid();
}
