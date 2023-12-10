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
                Payments = new List<Payment>()
                {
                    new Payment()
                    {
                        AmountPaid = 420,
                    },
                    new Payment()
                    {
                        AmountPaid = 550,
                    }
                },
                Activities = new List<Activity>()
                {
                    new Activity()
                    {
                        HoursWorked = 12
                    },
                    new Activity()
                    {
                        HoursWorked = 1
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
