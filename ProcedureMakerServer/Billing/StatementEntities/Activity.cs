using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Utils;


namespace ProcedureMakerServer.Billing.StatementEntities;

public partial class Activity : EntityBase
{
    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; }

    public Guid BillingElementId { get; set; }
    public BillingElement BillingElement { get; set; }

    public string ActivityDescription { get; set; } = string.Empty;
    public decimal Quantity { get; set; } = 0;
}

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        _ = builder.HasOne(x => x.Invoice)
        .WithMany(x => x.Activities)
        .OnDelete(DeleteBehavior.NoAction);
    }
}