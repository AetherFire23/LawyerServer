using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Entities;
namespace ProcedureMakerServer.Billing.StatementEntities;



public partial class AccountStatement : EntityBase
{
    public Guid CaseId { set; get; }
    public Case? Case { get; set; }

    public Guid LawyerId { get; set; }
    public Lawyer? Lawyer { get; set; }

    public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();

}

public class AccountStatementConfiguration : IEntityTypeConfiguration<AccountStatement>
{
    public void Configure(EntityTypeBuilder<AccountStatement> builder)
    {

    }
}
