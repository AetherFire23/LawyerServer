using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Billing.StatementDtos;
namespace ProcedureMakerServer.Dtos;

public class CaseContextDto
{
    public UserDto User { get; set; }
    public LawyerDto Lawyer { get; set; }
    public List<CaseDto> Cases { get; set; } = new List<CaseDto>();
    public List<ClientDto> Clients { get; set; } = new List<ClientDto>();
}

