using EFCoreBase.Entities;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Entities.BaseEntities;
using ProcedureMakerServer.Enums;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Dtos;

[TsClass]
public class CaseDto : EntityBase
{
    public Lawyer ManagerLawyer { get; set; }
    public Client Client { get; set; }
    public List<CasePart> Participants { get; set; } = new List<CasePart>();


    // theoretically it should be by alphabetical order then 
    // theoreticalilwayne
    public CourtMemberBase Plaintiff => GetPlaintiffAndDefender().p;
    public CourtMemberBase Defender => GetPlaintiffAndDefender().d;
    public List<CasePart> NotifiableMembers => Participants.Where(x => x.IsNotifiable).ToList();

    public string DistrictName { get; set; } = string.Empty;
    public string CourtAffairNumber { get; set; } = string.Empty;
    public string CaseNumber { get; set; } = string.Empty; // cases can have different filenumbers if it comes again many times 
    public ChamberNames ChamberName { get; set; }
    public CourtTypes CourtTypes { get; set; }
    public int CourtNumber { get; set; }


    public string GetFormattedCaseNames()
    {
        string caseNames = $"{this.Defender.LowerCaseFormattedFullName} c. {this.Plaintiff.LowerCaseFormattedFullName}";
        return caseNames;
    }

    // grosse note : quand y va avoir des defenderesses en garantie n shit y va y en avoir plusieurs....
    // va devenir complique de toute demele et faut jpose des questions
    public (CourtMemberBase p, CourtMemberBase d) GetPlaintiffAndDefender()
    {


        if (Client.CourtRole is CourtRoles.Plaintiff)
        {
            var defender = Participants.FirstOrDefault(x => x.CourtRole == CourtRoles.Defender);
            return (Client, defender);
        }

        var plaintiff = Participants.FirstOrDefault(x => x.CourtRole == CourtRoles.Defender);

        return (plaintiff, Client);
    }
}
