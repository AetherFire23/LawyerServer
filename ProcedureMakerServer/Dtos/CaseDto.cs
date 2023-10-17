using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Enums;

namespace ProcedureMakerServer.Dtos;

public class CaseDto : EntityBase
{
    public Lawyer ManagerLawyer { get; set; } = new Lawyer(); 
    public Client Client { get; set; } = new Client(); 
    public List<CasePart> Participants { get; set; } = new List<CasePart>(); 

    public string ClientCase { get; set; } = string.Empty; 
    public string ClientCaseReccurent { get; set; } = string.Empty; // for when more than 1 case is with the same client
    public CourtType CourtType { get; set; } 
    public string DistrictName { get; set; } = string.Empty; 
    public string CourtAffairNumber { get; set; } = string.Empty; 
}
