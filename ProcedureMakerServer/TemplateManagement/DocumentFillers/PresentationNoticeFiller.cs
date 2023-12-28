using DocumentFormat.OpenXml.Packaging;
using ProcedureShared.Dtos;
using System.Text;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.PresentationNotice)]
public class PresentationNoticeFiller : DocumentFillerBase
{
	public override string FormatEmailSubjectTitle(CaseDto dto)
	{
		StringBuilder builder = new StringBuilder();
		_ = builder.Append("NOTIFICATION PAR COURRIEL ");
		_ = builder.Append($"({dto.CourtNumber}) ");
		_ = builder.Append(dto.Plaintiff.LowerCaseFormattedFullName ?? "");
		_ = builder.Append(" c. ");
		_ = builder.Append(dto.Plaintiff.LowerCaseFormattedFullName ?? "");
		_ = $"NOTIFICATION PAR COURRIEL ({dto.CourtNumber}) {dto.Defender.LowerCaseFormattedFullName} c. {dto.Plaintiff.LowerCaseFormattedFullName}";
		return builder.ToString();
	}

	// expects
	protected override void CreateFixedReplacementKeywords(CaseDto caseDto, List<(string From, string To)> keywordMap, object? additional = null)
	{

	}

	protected override void FillArrayFields(CaseDto caseDto, WordprocessingDocument document, object? additional = null)
	{

		//  Console.WriteLine(additional.GetType());
	}
}
