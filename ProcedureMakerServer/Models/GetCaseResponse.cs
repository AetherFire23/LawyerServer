using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Models;

[TsClass]
public class GetCaseResponse
{
    public Guid CreatedId { get; set; } = Guid.Empty;
}
