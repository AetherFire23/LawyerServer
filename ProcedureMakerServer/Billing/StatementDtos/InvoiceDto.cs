using EFCoreBase.Entities;
using ProcedureMakerServer.Billing.InvoiceDtos;

namespace ProcedureMakerServer.Billing.StatementDtos;



public class InvoiceDto : EntityBase
{
    public InvoiceStatuses InvoiceStatus { get; set; } = InvoiceStatuses.InPreparation;
    public List<ActivityEntryDto> Activities { get; set; } = new();
    public List<PaymentDto> Payments { get; set; } = new();

    public void AddPayment(decimal amount)
    {
        PaymentDto payment = new PaymentDto()
        {
            AmountPaid = amount,
            AmountPaidDate = DateTime.UtcNow,
        };

        Payments.Add(payment);
    }
}
