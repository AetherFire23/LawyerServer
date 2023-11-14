using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Billing;
using ProcedureMakerServer.Billing.Services;

namespace ProcedureMakerServer.Controllers;


[ApiController]
[Route("[controller]")]
public class InvoiceController : Controller
{
    private IBillingService _billingService { get; set; }

    public InvoiceController(IBillingService billingService)
    {
        _billingService = billingService;
    }

    [HttpPost("updateinvoices")]
    public async Task<IActionResult> UpdateInvoice([FromBody] InvoiceDto accountStatement)
    {
        await _billingService.UpdateInvoice(accountStatement);

        return Ok();
    }

    [HttpPost("addactivity")]
    public async Task<IActionResult> AddActivity([FromBody] ActivityCreation activityCreation)
    {
        await _billingService.AddActivityToInvoice(activityCreation);

        return Ok();
    }

    [HttpPost("addbillingelement")]
    public async Task<IActionResult> AddBillingElement([FromBody] BillingElementCreationRequest billingElementCreation)
    {
        await _billingService.AddBillingElement(billingElementCreation);

        return Ok();
    }

    [HttpPost("addinvoice")]
    public async Task<IActionResult> AddInvoice(Guid caseId)
    {
        await _billingService.AddInvoice(caseId);
        return Ok();
    }

    [HttpPost("addpayment")]
    public async Task<IActionResult> AddPayment([FromBody] PaymentCreationRequest paymentCreation)
    {
        await _billingService.AddPayment(paymentCreation);
        return Ok();
    }

    // deletes invoices on case deletion only. 

    [HttpGet("getaccountstatement")]
    public async Task<IActionResult> GetAccountStatement(Guid caseId)
    {
        var statement = await _billingService.MapAccountStatementDto(caseId);
        return Ok(statement);
    }
}
