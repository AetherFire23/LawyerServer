namespace ProcedureMakerServer.Billing.StatementEntities;


// join table
public class AccountStatementBillingElement
{
    public Guid AccountStatementId { get; set; }
    public AccountStatement AccountStatement { get; set; }

    public Guid BillingElementId { get; set; }
    public BillingElement BillingElement { get; set; }
}
