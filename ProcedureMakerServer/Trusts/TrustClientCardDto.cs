using EFCoreBase.Entities;
namespace ProcedureMakerServer.Trusts;

public class TrustClientCardDto : EntityBase
{
    public Guid ClientId { get; set; }
    public List<TrustPaymentDto> Payments { get; set; } = new();

    // Can only Withdraw wen an INVOICE is made !
    public List<TrustWithdrawDto> Withdraws { get; set; } = new();

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
        Withdraws.Add(new TrustWithdrawDto()
        {
            Amount = amount,
            Date = DateTime.UtcNow,
        });
    }

    public decimal GetTrustBalance()
    {
        decimal totalPayments = Payments.Sum(x => x.Amount);
        decimal totalDisburses = Withdraws.Sum(x => x.Amount);
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

public class TrustWithdrawDto : EntityBase
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
