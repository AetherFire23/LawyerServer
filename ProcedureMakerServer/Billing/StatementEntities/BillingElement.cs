﻿using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Billing.StatementEntities;


// All billing elements will be global - accessible to the lawyer at all times.
// However some invoices will have a specific, restricted billing element.
public class BillingElement : EntityBase
{
    public Guid ManagerLawyerId { get; set; }
    public Lawyer ManagerLawyer { get; set; }
    public string ActivityName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    public bool IsHourlyRate { get; set; } = true;
    public bool IsDisburse { get; set; } = false; // important for UI and classifcaiont considerations 

    public Guid SpecificInvoiceId { get; set; }
    public bool IsInvoiceSpecific { get; set; } = false;

    public BillingElementDto ToDto()
    {
        var element = new BillingElementDto()
        {
            Id = this.Id,
            ActivityName = ActivityName,
            Amount = Amount,
            IsHourlyRate = IsHourlyRate,
            IsDisburse = IsDisburse,
        };

        return element;
    }
}

public class BillingElementDto : EntityBase
{
    public string ActivityName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    public bool IsHourlyRate { get; set; } = true;
    public bool IsDisburse { get; set; } = false;

}

public class BillingElementConfiguration : IEntityTypeConfiguration<BillingElement>
{
    public void Configure(EntityTypeBuilder<BillingElement> builder)
    {
    }
}