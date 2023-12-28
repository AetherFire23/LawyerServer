using EFCoreBase.Entities;
using ProcedureMakerServer.Entities.BaseEntities;
using ProcedureMakerServer.Trusts;
using System.Diagnostics.Contracts;

namespace ProcedureMakerServer.Dtos;

public class ClientDto : CourtMemberBase
{

    // should I keep guids >?
    public TrustClientCardDto TrustClientCard { get; set; }
    public List<CaseDto> Cases { get; set; } = new List<CaseDto>();
}
