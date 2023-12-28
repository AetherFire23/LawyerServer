using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.Backing)]
public class BackingFiller : DocumentFillerBase
{
    //private readonly 
    //public BackingFiller()
    //{
        
    //}

    public override string FormatEmailSubjectTitle(CaseDto dto)
    {
        return "";
    }

    // should use dictionary maybe?
    protected override void CreateFixedReplacementKeywords(CaseDto caseDto, List<(string From, string To)> keywordMap, object? additional = null)
    {
        if (additional is null) throw new Exception("params cant be null");


        BackingFillerParams parms = additional as BackingFillerParams;

        keywordMap.Add(("courtAffairNumber", $"{caseDto.CourtAffairNumber}"));
        keywordMap.Add(("courtName", $"{caseDto.CourtTypes}"));
        keywordMap.Add(("chamberName", $"{caseDto.ChamberName}"));

        keywordMap.Add(("plaintiffName", $"{caseDto.Plaintiff.FirstName}"));
        keywordMap.Add(("genderedPlaintiff", $"{caseDto.Plaintiff.FullName}"));
        keywordMap.Add(("defenderName", $"{caseDto.Defender.FullName.ToUpper()}"));
        keywordMap.Add(("genderedDefender", $"{caseDto.Defender.GetGenderedCourtRoleName()}"));
        keywordMap.Add(("documentName", $"{parms.DocumentName}"));
        keywordMap.Add(("lawyerName", $"{caseDto.ManagerLawyer.UppercaseFormattedFullName}"));
        keywordMap.Add(("fullAddress", $"{caseDto.ManagerLawyer.Address}"));
        keywordMap.Add(("postalCode", $"{caseDto.ManagerLawyer.PostalCode}"));
        keywordMap.Add(("fax", $"{caseDto.ManagerLawyer.Fax}"));
        keywordMap.Add(("notificationEmail", $"{caseDto.ManagerLawyer.NotificationEmail}"));
        keywordMap.Add(("lawyerCourtNumber", $"{caseDto.CourtAffairNumber}"));
        keywordMap.Add(("fileNumber", $"{caseDto.CaseNumber}"));
        keywordMap.Add(("courtLockerNumber", $"{caseDto.ManagerLawyer.CourtLockerNumber}"));
    }
}

public class BackingFillerParams
{
    public string DocumentName { get; set; }
}