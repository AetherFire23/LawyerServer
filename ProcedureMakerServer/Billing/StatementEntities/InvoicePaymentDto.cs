namespace ProcedureMakerServer.Billing.StatementEntities;

public class InvoicePaymentDto
{
    public Guid Id { get; set; }
    public decimal AmountPaid { get; set; }
    public DateTime? AmoundPaidDate { get; set; }
    public bool IsPaymentComingFromTrust { get; set; }
}
