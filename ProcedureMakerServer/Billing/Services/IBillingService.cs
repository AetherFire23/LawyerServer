namespace ProcedureMakerServer.Billing.Services;

public interface IBillingService
{
    Task<AccountStatement> GetAccountStatement(Guid caseId);
    Task UpdateAccountStatement(AccountStatement upToDateStatement);
}