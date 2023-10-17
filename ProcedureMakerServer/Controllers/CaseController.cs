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

    [HttpPost("createcase")]
    public async Task<IActionResult> CreateCaseContext([FromBody] CaseCreationInfo caseInfo)
    {
        await _caseContextService.CreateNewCase(caseInfo);

        return Ok();
    }

    public async Task<IActionResult> ModifyCaseContext([FromBody] CasesContext caseContext)
    {

    }
}
