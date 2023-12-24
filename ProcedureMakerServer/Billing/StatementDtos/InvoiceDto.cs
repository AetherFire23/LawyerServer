using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.StatementEntities;
namespace ProcedureMakerServer.Billing.StatementDtos;

public class InvoiceDto : EntityBase
{
    public InvoiceStatuses InvoiceStatus { get; set; } = InvoiceStatuses.InPreparation;
    public List<ActivityDto> Activities { get; set; } = new();
    public List<InvoicePaymentDto> Payments { get; set; } = new();
}
