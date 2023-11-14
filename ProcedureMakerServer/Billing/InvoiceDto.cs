using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing;


public class InvoiceDto : EntityBase
{

    // billing is not management by activity. It is managed by statement.
    public InvoiceStatuses InvoiceStatus { get; set; } = InvoiceStatuses.InPreparation;
    public List<ActivityDto> Activities { get; set; } = new List<ActivityDto>();
    public List<Payment> Payments { get; set; } = new List<Payment>();


}
