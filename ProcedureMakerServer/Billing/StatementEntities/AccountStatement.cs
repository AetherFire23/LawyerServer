using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Entities;
namespace ProcedureMakerServer.Billing.StatementEntities;

public partial class AccountStatement : EntityBase
{
	public Guid CaseId { set; get; }
	public Case Case { get; set; }
	public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();
	public Lawyer Lawyer => this.Case.ManagerLawyer;
}

public class AccountStatementConfiguration : IEntityTypeConfiguration<AccountStatement>
{
	public void Configure(EntityTypeBuilder<AccountStatement> builder)
	{

	}
}
