using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;
using System.Security.Claims;


namespace ProcedureMakerServer.Controllers;
[ApiController]
[Route("[controller]")]
public class CaseController : Controller
{
    private readonly ICaseContextService _caseContextService;
    private readonly ILawyerRepository _lawyerRepository;
    private readonly ProcedureContext _procedureContext;

    public CaseController(ICaseContextService caseContextService,
        ILawyerRepository lawyerRepository, ProcedureContext procedureContext)
    {
        _caseContextService = caseContextService;
        _lawyerRepository = lawyerRepository;
        _procedureContext = procedureContext;
    }

    // I wanna create 
    [HttpPost(CasesEndpoints.CreateNewCase)]
    public async Task<IActionResult> CreateCaseContext([FromBody] CaseCreationInfo caseInfo)
    {
        GetCaseResponse createdId = await _caseContextService.CreateNewCase(caseInfo);

        return Ok(createdId);
    }

    [HttpGet(CasesEndpoints.GetCasesContext)]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<IActionResult> GetCaseContext()
    {

        Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Guid lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;

        if (lawyerId.Equals(Guid.Empty)) throw new ArgumentInvalidException("lawyer id null");
        CasesContext caseContext = await _caseContextService.GetCaseContext(lawyerId);
        Console.WriteLine("case hit");
        return Ok(caseContext);
    }

    //[HttpGet("getcasestoken")]
    //public async Task<IActionResult> GetCaseContext()
    //{
    //    Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    //    Guid lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;

    //    if (lawyerId.Equals(Guid.Empty)) throw new ArgumentInvalidException("lawyer id null");
    //    CasesContext caseContext = await _caseContextService.GetCaseContext(lawyerId);
    //    return Ok(caseContext);
    //}

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
