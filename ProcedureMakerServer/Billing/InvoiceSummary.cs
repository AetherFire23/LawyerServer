using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Dtos;
namespace ProcedureMakerServer.Billing;

public class InvoiceSummary
{
    public CaseDto Case { get; set; }
    public InvoiceDto Invoice { get; set; }

    public int BillNumber { get; set; }
    public DateTime? BillActivityStartDate { get; set; }
    public DateTime? BillEndingDate { get; set; }
    public decimal SubTotal { get; set; } = decimal.Zero;
    public decimal TPSTax { get; set; }
    public decimal TVQTax { get; set; }
    public decimal Total { get; set; }
    public decimal Balance { get; set; } // without payments
}
