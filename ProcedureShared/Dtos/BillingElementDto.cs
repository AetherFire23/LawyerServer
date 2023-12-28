using EFCoreBase.Entities;

namespace ProcedureShared.Dtos;

public class BillingElementDto : EntityBase
{
	public string ActivityName { get; set; } = string.Empty;
	public decimal Amount { get; set; } = 0;
	public bool IsHourlyRate { get; set; } = true;
	public bool IsDisburse { get; set; } = false;

}