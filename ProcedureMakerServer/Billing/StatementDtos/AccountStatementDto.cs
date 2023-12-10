using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;

namespace ProcedureMakerServer.Billing;

public class AccountStatementDto : EntityBase
{
    public List<InvoiceDto> Invoices { get; set; } = new List<InvoiceDto>();

    public List<BillingElementDto> AvailableBillingElements = new List<BillingElementDto>();


    public Guid GetBillingElementIdByName(string name)
    {
        Guid id = this.AvailableBillingElements.FirstOrDefault(x => x.Name == name).Id;
        return id;
    }
}
