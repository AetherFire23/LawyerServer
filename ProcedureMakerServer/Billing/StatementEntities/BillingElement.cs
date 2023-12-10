using EFCoreBase.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.Billing.StatementEntities;


// will be constant
public class BillingElement : EntityBase, ListExtensions.ICopyFromAbleDto<BillingElementDto>
{
    // consider optional one to one relationship
    public Guid AccountStatementGuid { get; set; } = Guid.Empty;
    public AccountStatement AccountStatement { get; set; }

    // if personalized the AccountStatementDto will sort it in a different list.
    public bool IsPersonalizedBillingElement { get; set; } = false;
    public string ActivityName { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0;
    public bool IsHourlyRate { get; set; } = true;

    public BillingElement()
    {
        
    }

    public BillingElement(BillingElementDto dto, AccountStatement trackedAccountStatement)
    {
        this.CopyFromDto(dto);
        this.AccountStatement = trackedAccountStatement;
    }
    public BillingElementDto ToDto()
    {
        var dto = new BillingElementDto()
        {
            Id = Id,
            Name = ActivityName,
            Cost = Amount,
            IsHourlyRate = IsHourlyRate,
            IsPersonalizedBillingElement = IsPersonalizedBillingElement
        };
        return dto;
    }

    public void CopyFromDto(BillingElementDto billingElementDto)
    {
        this.ActivityName = billingElementDto.Name;
        this.Amount = billingElementDto.Cost;
        this.IsHourlyRate = billingElementDto.IsHourlyRate;
        this.IsPersonalizedBillingElement = billingElementDto.IsPersonalizedBillingElement;
    }
}

public class BillingElementDto : EntityBase
{
    public bool IsPersonalizedBillingElement { get; set; } = false;
    public string Name { get; set; } = string.Empty;
    public decimal Cost { get; set; } = 0;
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