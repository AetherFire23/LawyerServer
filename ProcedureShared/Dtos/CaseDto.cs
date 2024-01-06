using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Enums;
using ProcedureShared.Entities.BaseEntities;
namespace ProcedureShared.Dtos;

public class CaseDto : EntityBase
{
	// honestly manager lawyer is not very necessary...
	public LawyerDto ManagerLawyer { get; set; }
	public ClientDto Client { get; set; }
	public List<CaseParticipantDto> Participants { get; set; } = new List<CaseParticipantDto>();
	public CourtMemberBase Defender { get; set; }
	public CourtMemberBase Plaintiff { get; set; }
	public List<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
	public CourtRoles ClientRoleInCase { get; set; } = CourtRoles.Defender;
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
		if (this.ClientRoleInCase is CourtRoles.Plaintiff)
		{
			CaseParticipantDto? defender = Participants.FirstOrDefault(x => x.CourtRole == CourtRoles.Defender);
			return (Client, defender);
		}

		CaseParticipantDto? plaintiff = Participants.FirstOrDefault(x => x.CourtRole == CourtRoles.Defender);

		return (plaintiff, Client);
	}
}
