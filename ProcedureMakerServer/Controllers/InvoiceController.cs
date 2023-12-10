using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Billing.Services;
using ProcedureMakerServer.Trusts;
namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : Controller
{
    private readonly AccountStatementRepository _accountStatementRepository;
    private readonly TrustRepository _trustRepository;
    

    public InvoiceController(AccountStatementRepository accountStatementRepository, TrustRepository trustRepository)
    {
        _accountStatementRepository = accountStatementRepository;
        _trustRepository = trustRepository;
    }

    [HttpGet("getaccountdto")]
    public async Task<IActionResult> GetAccountDto(Guid clientId)
    {
        var accountStatementDto = await _accountStatementRepository.ConstructAccountStatementDtoByCaseId(clientId);
        return Ok(accountStatementDto);
    }

    // trust
    [HttpGet("gettrustdto")]
    public async Task<IActionResult> GetTrustDto(Guid clientId)
    {
        var trustDto = await _trustRepository.ConstructTrustDto(clientId);
        return Ok(trustDto);
    }

    [HttpPost]
    public async Task<IActionResult> AddTrustPayment([FromBody] TrustPaymentDto trustPayment, Guid clientId)
    {
        await _trustRepository.AddPayment(clientId, trustPayment);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> ModifyPayment(Guid clientId, TrustPaymentDto trustPayment)
    {
        
        return Ok();
    }
}
