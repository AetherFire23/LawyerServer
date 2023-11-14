using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing;

public class AccountStatementDto : EntityBase
{
    public List<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();
    public List<BillingElementDto> LawyerBillingElements { get; set; } = new List<BillingElementDto>();
}
