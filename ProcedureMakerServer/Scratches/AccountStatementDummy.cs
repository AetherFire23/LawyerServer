using ProcedureMakerServer.Billing;

namespace ProcedureMakerServer.Scratches;

public class AccountStatementDummy
{
    public static AccountStatement CreateStatementToUpdate(Guid id)
    {
        List<Invoice> invoices = new List<Invoice>()
        {
            new Invoice()
            {
                Payments = new List<Payment>()
                {
                    new Payment()
                    {
                        AmountPaid = 420,
                    }
                }
            }
        };

        AccountStatement account = new AccountStatement()
        {
            Id = id,
            Invoices = invoices,
        };

        return account;
    }
}
