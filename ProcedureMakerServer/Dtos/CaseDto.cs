using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Enums;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Dtos;

[TsClass]
public class CaseDto : EntityBase
{
    public Lawyer ManagerLawyer { get; set; } = new Lawyer();
    public Client Client { get; set; } = new Client();
    public List<CasePart> Participants { get; set; } = new List<CasePart>();



    public string DistrictName { get; set; } = string.Empty;
    public string CourtAffairNumber { get; set; } = string.Empty;
    public string CaseNumber { get; set; } = string.Empty; // cases can have different filenumbers if it comes again many times 
    public CourtType CourtType { get; set; }
    public int CourtNumber { get; set; }
}
