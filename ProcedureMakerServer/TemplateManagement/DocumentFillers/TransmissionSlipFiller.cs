using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using System.Globalization;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.TransmissionSlip)]
public class TransmissionSlipFiller : DocumentFillerBase
{

    //private static 
    public override string FormatEmailSubjectTitle(CaseDto dto)
    {
        return "";
    }


    protected override void CreateFixedReplacementKeywords(CaseDto caseDto, List<(string From, string To)> keywordMap, object? additional = null)
    {
        ArgumentNullException.ThrowIfNull(additional);

        NotificationSlipParams additionalParams = additional as NotificationSlipParams;

        CultureInfo fr = new CultureInfo("fr-FR");
        keywordMap.Add(("dayMonthYear", DateTime.Now.ToString("dd/MMMM/yyyy", fr)));


        keywordMap.Add(("lawyerName", caseDto.ManagerLawyer.FullName));
        keywordMap.Add(("lawyerFileNumber", caseDto.CaseNumber));
        keywordMap.Add(("lawyerEmail", caseDto.ManagerLawyer.Email));
        keywordMap.Add(("lawyerNotificationEmail", caseDto.ManagerLawyer.NotificationEmail));
        keywordMap.Add(("faxNumber", caseDto.ManagerLawyer.Fax));
        keywordMap.Add(("phoneNumber", caseDto.ManagerLawyer.WorkPhoneNumber));
        keywordMap.Add(("courtNumber", caseDto.CourtAffairNumber)); // notification phone number?


        // Osti de calisse de marde:
        // quand tu fais un number ca fait jumper le number dans un run different pour une crisse de raison
        // faque chu mieux de mapper les members a une lettre
        // notifieda, notifiedb, notifiedc, notifiedb
        // for all notifaible members, add their name and email
        for (int i = 0; i < 4; i++)
        {
            string iteratedNotifiedIdentifier = $"notified{IndexToLetter(i)}";
            string iteratedEmailReplacer = $"notifiedEmail{IndexToLetter(i)}";


            bool isAccessibleNotifiableMembers = i < caseDto.GetNotifiableEmails().Count;
            if (isAccessibleNotifiableMembers)
            {
                var iteratedMember = caseDto.GetNotifiableParticipants()[i];

                keywordMap.Add((iteratedNotifiedIdentifier, iteratedMember.FirstName));

                keywordMap.Add((iteratedEmailReplacer, iteratedMember.NotificationEmail));
                continue;
            }

            // replace with nothing if over the notifiabler members count
            // this is because lbireoffice has 4 hardcodes slots, so its not a field array 

            keywordMap.Add((iteratedNotifiedIdentifier, string.Empty));
            keywordMap.Add((iteratedEmailReplacer, string.Empty));
        }

        keywordMap.Add(("formattedPartsNames", caseDto.GetFormattedCaseNames()));
        keywordMap.Add(("documentName", additionalParams.DocumentName));
        keywordMap.Add(("pageCount", additionalParams.PageCount.ToString()));

        //keywordMap.Add(($"Email", string.Empty));

    }

    private string IndexToLetter(int index)
    {
        char letter = (char)('a' + index);
        return letter.ToString();
    }
}

public class NotificationSlipParams
{
    public int PageCount { get; set; }
    public string DocumentName { get; set; } = string.Empty;
}