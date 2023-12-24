using ProcedureMakerServer.Billing.StatementEntities;

namespace ProcedureMakerServer.Scratches;

public class AccountStatementDummy
{
    public static AccountStatement CreateStatementToUpdate(Guid id)
    {
        List<Invoice> invoices = new List<Invoice>()
        {
            new Invoice()
            {
                Payments = new List<InvoicePayment>()
                {
                    new InvoicePayment()
                    {
                        AmountPaid = 420,
                    },
                    new InvoicePayment()
                    {
                        AmountPaid = 550,
                    }
                },
                Activities = new List<Activity>()
                {
                    new Activity()
                    {
                        Quantity = 12
                    },
                    new Activity()
                    {
                        Quantity = 1
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
