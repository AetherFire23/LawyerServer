namespace ProcedureMakerServer.Billing.Services;

public interface IBillingService
{
    Task AddActivityToInvoice(ActivityCreation activityCreation);
    Task AddBillingElement(BillingElementCreationRequest billingElementCreation);
    Task AddInvoice(Guid caseId);
    Task AddPayment(PaymentCreationRequest paymentCreation);
    Task<AccountStatementDto> MapAccountStatementDto(Guid caseId);
    Task UpdateInvoice(InvoiceDto updatedInvoice);
}