using EFCoreBase.Entities;
using Newtonsoft.Json;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Entities.BaseEntities;
using ProcedureMakerServer.Enums;
namespace ProcedureMakerServer.Dtos;

public class CaseDto : EntityBase
{
    public Lawyer ManagerLawyer { get; set; }
    public Client Client { get; set; }
    public List<CaseParticipantDto> Participants { get; set; } = new List<CaseParticipantDto>();

    // theoretically it should be by alphabetical order then 


    [JsonIgnore]
    public CourtMemberBase? Plaintiff => GetPlaintiffAndDefender().p;

    [JsonIgnore]
    public CourtMemberBase? Defender => GetPlaintiffAndDefender().d;

    //public List<CaseParticipant> NotifiableMembers => Participants.Where(x => x.IsNotifiable).ToList();

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

    public List<CaseParticipantDto> GetNotifiableParticipants()
    {
        var participants = this.Participants
            .Where(x => x.NotificationEmail != string.Empty)
            .Where(x => x.MustNotify)
            .ToList();
        return participants;
    }

    public List<string> GetNotifiableEmails()
    {
        var emails = this.GetNotifiableParticipants()
            .Select(x => x.NotificationEmail)
            .ToList();

        return emails;
    }

    // grosse note : quand y va avoir des defenderesses en garantie n shit y va y en avoir plusieurs....
    // va devenir complique de toute demele et faut jpose des questions
    public (CourtMemberBase? p, CourtMemberBase? d) GetPlaintiffAndDefender() 
    {
        if (Client.CourtRole is CourtRoles.Plaintiff)
        {
            CaseParticipantDto? defender = Participants.FirstOrDefault(x => x.CourtRole == CourtRoles.Defender);
            return (Client, defender);
        }

        CaseParticipantDto? plaintiff = Participants.FirstOrDefault(x => x.CourtRole == CourtRoles.Defender);

        return (plaintiff, Client);
    }
}
