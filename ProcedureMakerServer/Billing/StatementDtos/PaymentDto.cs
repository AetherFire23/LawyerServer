using EFCoreBase.Entities;

namespace ProcedureMakerServer.Billing.StatementDtos;

public class PaymentDto : EntityBase
{

    public decimal AmountPaid { get; set; }
    public DateTime? AmountPaidDate { get; set; }
}

