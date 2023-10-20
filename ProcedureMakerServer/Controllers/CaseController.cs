using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CaseController : Controller
{
    private readonly ICaseContextService _caseContextService;

    public CaseController(ICaseContextService caseContextService)
    {
        _caseContextService = caseContextService;
    }

    [HttpPost("createcase1")]
    public async Task<IActionResult> CreateCaseContext([FromBody] CaseCreationInfo caseInfo)
    {
        await _caseContextService.CreateNewCase(caseInfo);

        return Ok();
    }

    [HttpPut("createcase2")]
    public async Task<IActionResult> ModifyCaseContext([FromBody] CasesContext caseContext)
    {
        await _caseContextService.SaveContextDto(caseContext);
        return Ok();
    }

    [HttpGet("createcase3")]
    public async Task<IActionResult> GetCaseContext(Guid lawyerId)
    {
        var caseContext = await _caseContextService.GetCase(lawyerId);
        return Ok(caseContext);
    }

    [HttpGet("createcase4")]
    public async Task<IActionResult> DoStuff()
    {
        var s2 = new List<CaseDto>();

        var context = new CasesContext()
        {
            Cases = s2,
        };

        return Ok(context);
    }
}
