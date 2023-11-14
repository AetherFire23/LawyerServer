using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Billing;

public class InvoiceSummary
{
    public CaseDto Case { get; set; } = new CaseDto();
    public int BillNumber { get; set; }
    
    public DateTime? BillDate { get; set; }
    public DateTime? BillEndingDate { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalHours { get; set; }

    public List<ActivityDto> Activities { get; set; } = new List<ActivityDto>();

}
