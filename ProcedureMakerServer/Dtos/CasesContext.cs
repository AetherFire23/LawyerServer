using ProcedureMakerServer.Entities;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Dtos;


[TsClass]
public class CasesContext
{
    public List<CaseDto> Cases { get; set; } = new List<CaseDto>();

    public List<Client> Clients =>
        !Cases.Any()
        ? new List<Client>()
        : Cases.Select(x => x.Client).ToList();
}
