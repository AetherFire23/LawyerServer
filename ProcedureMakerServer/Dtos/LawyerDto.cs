using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Entities.BaseEntities;
namespace ProcedureMakerServer.Dtos;

public class LawyerDto : CourtMemberBase
{
    public string CourtLockerNumber { get; set; } = string.Empty;
    public int BillsEmittedCount { get; set; }
    public BillingElementDto DefaultHourlyElement { get; set; }
    public List<BillingElementDto> BillingElements { get; set; } = new List<BillingElementDto>();
}
