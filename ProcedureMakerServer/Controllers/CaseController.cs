using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;


namespace ProcedureMakerServer.Controllers;
[ApiController]
[Route("[controller]")]
public class CaseController : Controller
{
    private readonly ICaseContextService _caseContextService;
    private readonly ILawyerRepository _lawyerRepository;

    public CaseController(ICaseContextService caseContextService, ILawyerRepository lawyerRepository)
    {
        _caseContextService = caseContextService;
        _lawyerRepository = lawyerRepository;
    }


    // I wanna create 
    [HttpPost(CasesEndpoints.CreateNewCase)]
    public async Task<IActionResult> CreateCaseContext([FromBody] CaseCreationInfo caseInfo)
    {
        GetCaseResponse createdId = await _caseContextService.CreateNewCase(caseInfo);

        return Ok(createdId);
    }

    [HttpGet(CasesEndpoints.GetCasesContext)]
    public async Task<IActionResult> GetCaseContext(Guid lawyerId)
    {
        if (lawyerId.Equals(Guid.Empty)) throw new ArgumentInvalidException("lawyer id null");
        CasesContext caseContext = await _caseContextService.GetCaseContext(lawyerId);
        return Ok(caseContext);
    }

    [HttpPut(CasesEndpoints.SaveContextDto)]
    public async Task<IActionResult> ModifyCaseContext([FromBody] CaseDto caseDto)
    {
        await _caseContextService.SaveCaseDto(caseDto);
        return Ok();
    }


    [HttpPut("modifylawyer")]
    public async Task<IActionResult> ModifyLawyer([FromBody] Lawyer lawyer)
    {
        await _lawyerRepository.ModifyLawyer(lawyer);
        return Ok();
    }



    [HttpGet("authorizedrequest")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<IActionResult> AuthorizedRequest()
    {
        return Ok();
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
