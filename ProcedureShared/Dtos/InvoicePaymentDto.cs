using EFCoreBase.Entities;

namespace ProcedureShared.Dtos;

public class InvoicePaymentDto : EntityBase
{
	public decimal AmountPaid { get; set; }
	public DateTime? AmoundPaidDate { get; set; }
	public bool IsPaymentComingFromTrust { get; set; }
	public string Method { get; set; }
	
}
