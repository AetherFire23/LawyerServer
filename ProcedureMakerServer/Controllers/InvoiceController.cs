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
    public async Task<IActionResult> UpdateInvoice([FromBody] AccountStatement accountStatement)
    {
        await _billingService.UpdateAccountStatement(accountStatement);

        return Ok();
    }

    [HttpGet("getaccountstatement")]
    public async Task<IActionResult> GetAccountStatement(Guid caseId)
    {
        var statement = await _billingService.GetAccountStatement(caseId);
        return Ok(statement);
    }
}
