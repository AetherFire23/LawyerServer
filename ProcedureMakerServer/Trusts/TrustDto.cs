using EFCoreBase.Entities;

namespace ProcedureMakerServer.Trusts;
public class TrustDto : EntityBase
{
    public Guid ClientId { get; set; }
    public List<TrustPaymentDto> Payments { get; set; } = new();
    public List<TrustDisburseDto> Disburses { get; set; } = new();

    public void AddPayment(decimal amount)
    {
        Payments.Add(new TrustPaymentDto()
        {
            Amount = amount,
            Date = DateTime.UtcNow,
        });
    }

    public void AddDisburse(decimal amount)
    {
        Disburses.Add(new TrustDisburseDto()
        {
            AmountPaid = amount,
            Date = DateTime.UtcNow,
        });
    }

    public decimal GetTrustBalance()
    {
        decimal totalPayments = Payments.Sum(x => x.Amount);
        decimal totalDisburses = Disburses.Sum(x => x.AmountPaid);
        decimal balance = totalPayments - totalDisburses;
        return balance;
    }

    public bool CanPayForAmount(decimal amount)
    {
        bool canPay = amount >= GetTrustBalance();
        return canPay;
    }


}


public class TrustPaymentDto : EntityBase
{
    public decimal Amount { get; set; }
    public DateTime? Date { get; set; }

}

public class TrustDisburseDto : EntityBase
{
    public decimal AmountPaid { get; set; }
    public DateTime Date { get; set; }



}

