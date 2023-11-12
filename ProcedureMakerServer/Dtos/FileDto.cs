using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Entities.BaseEntities;
using ProcedureMakerServer.Enums;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Dtos;

[TsClass]
public class FileDto
{
    public string CaseNumber { get; set; } = string.Empty;
    public ChamberNames CourtType { get; set; }
    public Client CurrentLawyersClient { get; set; } = new Client();
    public List<CourtMemberBase> CourtMembers { get; set; } = new();


}
