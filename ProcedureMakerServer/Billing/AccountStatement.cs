using EFCoreBase.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcedureMakerServer.Billing;



public partial class AccountStatement : EntityBase
{
    public Guid CaseId {  set; get; }
    public Case? Case { get; set; }
    public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();

}
