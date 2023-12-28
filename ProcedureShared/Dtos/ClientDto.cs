using ProcedureMakerServer.Trusts;
using ProcedureShared.Entities.BaseEntities;

namespace ProcedureShared.Dtos;

public class ClientDto : CourtMemberBase
{
	// should I keep guids >?
	public TrustClientCardDto TrustClientCard { get; set; }
	public List<CaseDto> Cases { get; set; } = new List<CaseDto>();
}
