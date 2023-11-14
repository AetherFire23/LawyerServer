using EFCoreBase.Entities;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Entities;

namespace ProcedureMakerServer.Billing;


// will be constant
public class BillingElement : EntityBase
{
    // could Either be linked to a lawyer OR to a AccountStatement
    public Guid LawyerId { get; set; }
    public Lawyer Lawyer { get; set; }


    public Guid LawyerBillingOptionsId { get; set; }
    public LawyerBillingOptions? LawyerBillingOptions { get; set; }

    public Guid AccountStatementGuid { get; set; } = Guid.Empty;


    // if personalized the AccountStatementDto will sort it in a different list.
    public bool IsPersonalizedBillingElement { get; set; } = false;
    public string ActivityName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    public bool IsHourlyRate { get; set; } = true;

    public BillingElementDto ToDto()
    {
        var dto = new BillingElementDto()
        {
            Id = this.Id,
            ActivityName = this.ActivityName,
            Amount = this.Amount,
            IsHourlyRate = this.IsHourlyRate,
            IsPersonalizedBillingElement = this.IsPersonalizedBillingElement
        };
        return dto;
    }
}

public class BillingElementDto : EntityBase
{
    public bool IsPersonalizedBillingElement { get; set; } = false;
    public string ActivityName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    public bool IsHourlyRate { get; set; } = true;
}

public class BillingElementCreationRequest : EntityBase
{

    public Guid AccountStatementId { get; set; }
    public bool IsPersonalizedBillingElement { get; set; } = false;
    public string ActivityName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    public bool IsHourlyRate { get; set; } = true;
}


public class BillingElementConfiguration : IEntityTypeConfiguration<BillingElement>
{
    public void Configure(EntityTypeBuilder<BillingElement> builder)
    {
    }
}