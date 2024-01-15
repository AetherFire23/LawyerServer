using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Billing.InvoiceDtos;
using ProcedureMakerServer.Billing.Services;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Trusts;
using ProcedureShared.Dtos;
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
        var accountStatementDto = await _accountStatementRepository.ConstructAccountStatementDtoByCaseId(clientId);
        return Ok(accountStatementDto);
    }

    [HttpGet("GetTrustClientCardDto/clientid={clientId}")]
    public async Task<ActionResult<TrustClientCardDto>> GetTrustClientCardDto(Guid clientId)
    {
        var trustDto = await _trustRepository.ConstrustTrustClientCard(clientId);
        return Ok(trustDto);
    }

    // CREATEINVOICE 
    // https://www.youtube.com/watch?v=JG5Rp2ypWE8 
    [HttpPost("CreateInvoice")] // test exists
    public async Task<ActionResult<Guid>> CreateInvoice([FromQuery] Guid caseId)
    {
        var createdId = await _invoiceRepository.CreateInvoice(caseId);
        return Ok(createdId);
    }

    [HttpPut("UpdateInvoice")]
    public async Task<ActionResult> UpdateInvoiceProperties([FromBody] InvoiceDto invoiceDto)
    {
        await _invoiceRepository.UpdateInvoiceProperties(invoiceDto);
        return Ok();
    }

    [HttpPut("ArchiveInvoice")]
    public async Task<ActionResult> ArchiveInvoice(Guid invoiceId)
    {
        await _invoiceRepository.ArchiveInvoice(invoiceId);
        return Ok();
    }

    // ACTIVITIES OF INVOICES
    [HttpPost("CreateActivity")]
    [ProducesResponseType(200, Type = typeof(Guid))]
    public async Task<ActionResult<Guid>> CreateActivity([FromQuery] Guid invoiceId, [FromQuery] bool isDisburse, [FromQuery] bool isTaxable)
    {
        var createdId = await _invoiceRepository.AddActivity(invoiceId, isDisburse, isTaxable);
        return Ok(createdId);
    }

    [HttpPut("UpdateActivity")]
    public async Task<ActionResult> UpdateActivity([FromBody] ActivityDto activityDto)
    {
        await _invoiceRepository.UpdateActivity(activityDto);
        return Ok();
    }

    [HttpPut("RemoveActivity")]
    public async Task<ActionResult> RemoveActivity([FromQuery] Guid activityId)
    {
        await _invoiceRepository.RemoveActivity(activityId);
        return Ok();
    }

    // MODIFY BILLING ELEMENTS :)
    [HttpPost("CreateBillingElement")]
    public async Task<ActionResult<Guid>> CreateBillingElement(Guid lawyerId)
    {
        var createdId = await _invoiceRepository.AddBillingElement(lawyerId);
        return Ok(createdId);
    }
    [HttpPost("UpdateBillingElement")]
    public async Task<ActionResult> UpdateBillingElement(BillingElementDto billingElementDto)
    {
        await _invoiceRepository.UpdateBillingElement(billingElementDto);
        return Ok();
    }
    [HttpPost("DeleteBillingElement")]
    public async Task<ActionResult> DeleteBillingElement(Guid billingElementId)
    {
        await _invoiceRepository.RemoveBillingElement(billingElementId);
        return Ok();
    }

    // TRUST ONLY
    [HttpPost("AddFundsToTrust")]
    public async Task<ActionResult> AddFundsToTrust([FromQuery] Guid clientId)
    {
        await _trustRepository.AddFundsToTrust(clientId);
        return Ok();
    }

    [HttpPut("UpdateTrustPayment")]
    public async Task<IActionResult> ModifyTrustPayment([FromBody] TrustPaymentDto updatedTrustPayment)
    {
        await _trustRepository.UpdateTrustFund(updatedTrustPayment);
        return Ok();
    }

    [HttpDelete("RemoveTrustPayment")]
    public async Task<ActionResult> RemoveTrustFund(Guid trustPaymentId)
    {
        await _trustRepository.RemoveTrustFund(trustPaymentId);
        return Ok();
    }

    // MODIFY PAYMENTS OF INVOICE  
    [HttpPut("AddInvoicePayment")]
    public async Task<ActionResult<Guid>> AddInvoicePayment([FromQuery] Guid invoiceId)
    {
        var createdId = await _invoiceRepository.AddInvoicePayment(invoiceId);
        return Ok(createdId);
    }

    [HttpPut("UpdateInvoicePayment")]
    public async Task<ActionResult> UpdateInvoicePayment([FromBody] InvoicePaymentDto invoicePayment)
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

    [HttpGet("GetInvoice")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    public async Task<ActionResult> DownloadInvoice(Guid invoiceId)
    {
        var fileBytes = await _invoiceRepository.GetInvoicePdfAsBytes(invoiceId);

        var virtualFile = File(fileBytes, "application/pdf");

        Response.ContentType = "application/pdf";
        return Ok(virtualFile);
    }

    // so this seems to work ? 
    [HttpGet("GetInvoice2")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    public async Task<IActionResult> DownloadInvoice2(Guid invoiceId)
    {
        var fileBytes = await _invoiceRepository.GetInvoicePdfAsBytes(invoiceId);
        var virtualFile = File(fileBytes, "application/pdf");



        return Ok(virtualFile);
    }
}
