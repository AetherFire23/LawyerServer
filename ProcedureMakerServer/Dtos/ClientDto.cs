using EFCoreBase.Entities;
using ProcedureMakerServer.Entities.BaseEntities;

namespace ProcedureMakerServer.Dtos;

public class ClientDto : CourtMemberBase
{

    public ICollection<Case> Cases { get; set; } = new List<Case>();
}
