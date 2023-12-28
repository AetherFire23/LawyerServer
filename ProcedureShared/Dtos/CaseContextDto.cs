
using ProcedureShared.Authentication;
using System.Text.Json.Serialization;

namespace ProcedureShared.Dtos;

public class CaseContextDto
{
	public UserDto User { get; set; }
	public LawyerDto Lawyer { get; set; }
	//public List<CaseDto> Cases { get; set; } = new List<CaseDto>();
	public List<ClientDto> Clients { get; set; } = new List<ClientDto>();

	[JsonIgnore]
	public List<CaseDto> Cases => !Clients.Any() ? new List<CaseDto>() : Clients.SelectMany(x => x.Cases).ToList();
}

