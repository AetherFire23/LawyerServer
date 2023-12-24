using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Billing.Services;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Billing.StatementEntities;
using ProcedureMakerServer.Trusts;
namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : Controller
{
    private readonly AccountStatementRepository _accountStatementRepository;
    private readonly TrustRepository _trustRepository;
    private readonly InvoiceRepository _invoiceRepository;
    public InvoiceController(AccountStatementRepository accountStatementRepository,
                             TrustRepository trustRepository,
                             InvoiceRepository invoiceRepository)
    {
        _accountStatementRepository = accountStatementRepository;
        _trustRepository = trustRepository;
        _invoiceRepository = invoiceRepository;
    }

    // GET TRUST OR INVOICE
    [HttpGet("account/clientId={clientId}")]
    public async Task<ActionResult<AccountStatementDto>> GetAccountStatementDto(Guid clientId)
    {
        AccountStatementDto accountStatementDto = await _accountStatementRepository.ConstructAccountStatementDtoByCaseId(clientId);
        return Ok(accountStatementDto);
    }

    [HttpGet("trust/clientid={clientId}")]
    public async Task<IActionResult> GetTrustDto(Guid clientId)
    {
        TrustDto trustDto = await _trustRepository.ConstructTrustDto(clientId);
        return Ok(trustDto);
    }

    // CREATEINVOICE 
    // https://www.youtube.com/watch?v=JG5Rp2ypWE8

    [HttpPost("CreateInvoice")]
    public async Task<ActionResult> CreateInvoice([FromQuery] Guid clientId)
    {
        await _invoiceRepository.CreateInvoice(clientId);
        return Ok();
    }

    [HttpPut("UpdateInvoice")]
    public async Task<ActionResult> UpdateInvoiceProperties(InvoiceDto invoiceDto)
    {
        await _invoiceRepository.UpdateInvoiceProperties(invoiceDto);
        return Ok();
    }
    [HttpPut("ArchiveInvoice")]
    public async Task<ActionResult> ArchiveInvoice(Guid invoicePaty)
    {
        return Ok();
    }

    // TRUST ONLY
    [HttpPost]
    public async Task<ActionResult> AddFundsToTrust([FromQuery] Guid clientId, [FromBody] TrustPaymentDto trustPayment)
    {
        await _trustRepository.AddFundsToTrust(clientId, trustPayment);
        return Ok();
    }
    [HttpPut]
    public async Task<IActionResult> ModifyTrustPayment([FromBody] TrustPaymentDto updatedTrustPayment)
    {
        await _trustRepository.UpdateTrustFund(updatedTrustPayment);
        return Ok();
    }
    [HttpDelete]
    public async Task<ActionResult> RemoveTrustFund(Guid trustPaymentId)
    {
        await _trustRepository.RemoveTrustFund(trustPaymentId);
        return Ok();
    }


    // MODIFY PAYMENTS OF INVOICE  

    [HttpPut("AddInvoicePayment")]
    public async Task<ActionResult> AddInvoicePayment(Guid invoiceId, InvoicePaymentDto invoicePayment)
    {
        await _invoiceRepository.AddInvoicePayment(invoiceId, invoicePayment);
        return Ok();
    }

    [HttpPut("UpdateInvoicePayment")]
    public async Task<ActionResult> UpdateInvoicePayment(InvoicePaymentDto invoicePayment)
    {
        await _invoiceRepository.UpdateInvoicePayment(invoicePayment);
        return Ok();
    }

    [HttpPut("RemoveInvoicePayment")]
    public async Task<ActionResult> RemoveInvoicePayment(Guid invoicePaymentId)
    {
        await _invoiceRepository.DeleteInvoicePayment(invoicePaymentId);
        return Ok();
    }
}
