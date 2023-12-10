using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.Billing;

public class InvoiceSummary
{
    public CaseDto Case { get; set; } = new CaseDto();
    public int BillNumber { get; set; }
    
    public DateTime? BillDate { get; set; }
    public DateTime? BillEndingDate { get; set; }
    public decimal Balance { get; set; }
    public decimal TotalHours { get; set; }

    public List<ActivityEntryDto> Activities { get; set; } = new List<ActivityEntryDto>();

}
