namespace ProcedureMakerServer.Billing.Services;

public interface IBillingService
{
    Task UpdateInvoices(AccountStatement upToDateStatement);
}