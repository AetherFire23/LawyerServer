using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureShared.Authentication;
using System.Text.Json.Serialization;
namespace ProcedureShared.Dtos;
public class CaseContextDto
{
    public UserDto User { get; set; }
    public LawyerDto Lawyer { get; set; }
    //public List<CaseDto> Cases { get; set; } = new List<CaseDto>();
    public List<ClientDto> Clients { get; set; } = new List<ClientDto>();

    [JsonIgnore]
    public List<CaseDto> Cases => !Clients.Any() ? new List<CaseDto>() : Clients.SelectMany(x => x.Cases).ToList();

    public List<InvoiceDto> GetInvoices()
    {
        var invoices = Clients.SelectMany(c => c.Cases.SelectMany(i => i.Invoices)).ToList();
        return invoices;
    }
    public InvoiceDto GetInvoice(Guid invoiceId)
    {
        var invoice = GetInvoices().First(i => i.Id == invoiceId);
        return invoice;
    }
}

