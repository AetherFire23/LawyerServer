using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Dtos;



public class CasesContext
{
    public Lawyer Lawyer { get; set; }
    public List<CaseDto> Cases { get; set; } = new List<CaseDto>();



    public List<Client> Clients =>
        !Cases.Any()
        ? new List<Client>()
        : Cases.Select(x => x.Client).ToList();
}
