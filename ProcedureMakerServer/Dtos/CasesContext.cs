using ProcedureMakerServer.Entities;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Dtos;


[TsClass]
public class CasesContext
{
    public Lawyer Lawyer { get; set; }
    public List<CaseDto> Cases { get; set; } = new List<CaseDto>();


    [TsIgnore]
    public List<Client> Clients =>
        !Cases.Any()
        ? new List<Client>()
        : Cases.Select(x => x.Client).ToList();
}
