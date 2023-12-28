using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Services;
using System.Security.Claims;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CaseController : Controller
{
    private readonly CaseContextService _caseContextService;
    private readonly LawyerRepository _lawyerRepository;
    private readonly ProcedureContext _procedureContext;
    private readonly ClientRepository _clientRepository;
    private readonly CasesContextRepository _casesContextRepository;
    public CaseController(CaseContextService caseContextService,
                          LawyerRepository lawyerRepository,
                          ProcedureContext procedureContext,
                          ClientRepository clientRepository,
                          CasesContextRepository casesContextRepository)
    {
        _caseContextService = caseContextService;
        _lawyerRepository = lawyerRepository;
        _procedureContext = procedureContext;
        _clientRepository = clientRepository;
        _casesContextRepository = casesContextRepository;
    }

    // Register lawyer
    // Create Client
    // Create Case (many cases for 1 client)
    // Edit client, edit case
    // I wanna create 

    [HttpGet(CasesEndpoints.GetCasesContext)]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult<CaseContextDto>> GetCaseContext()
    {
        Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Guid lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;

        if (lawyerId.Equals(Guid.Empty)) throw new ArgumentInvalidException("lawyer id null");
        CaseContextDto caseContext = await _caseContextService.GetCaseContext(lawyerId);
        Console.WriteLine("case hit");
        return Ok(caseContext);
    }

    [HttpPost("CreateNewCase")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult<GetCaseResponse>> CreateNewCase([FromBody] CaseCreationInfo caseInfo)
    {
        Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Guid lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;
        GetCaseResponse createdId = await _caseContextService.CreateNewCase(lawyerId, caseInfo);

        return Ok(createdId);
    }



    [HttpPut("savecase")]
    public async Task<ActionResult> UpdateCasePrimitiveProperties([FromBody] CaseDto caseDto)
    {
        await _caseContextService.UpdateCasePrimitiveProps(caseDto);
        return Ok();
    }

    [HttpPut("updatelawyer")]
    public async Task<ActionResult> UpdateLawyer([FromBody] LawyerDto lawyer)
    {
        await _lawyerRepository.UpdateLawyer(lawyer);
        return Ok();
    }

    [HttpPut("addclient")]
    public async Task<ActionResult> AddClient([FromBody] ClientDto newClient)
    {
        Guid userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Guid lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;
        await _clientRepository.CreateClient(lawyerId, newClient);
        return Ok();
    }

    [HttpGet("CreateCaseParticipant")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult> CreateCaseParticipant([FromBody] CaseParticipantDto caseParticipant, [FromQuery] Guid caseId)
    {
        await _casesContextRepository.AddCaseParticipant(caseId, caseParticipant);
        return Ok();
    }

    [HttpGet("UpdateCaseParticipant")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult> UpdateCaseParticipant([FromBody] CaseParticipantDto caseParticipant)
    {
        await _casesContextRepository.UpdateCaseParticipant(caseParticipant);
        return Ok();
    }

    [HttpDelete("RemoveCaseParticipant")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult> RemoveCaseParticipant([FromBody] Guid caseParticipantId)
    {
        await _casesContextRepository.RemoveCaseParticipant(caseParticipantId);
        return Ok();
    }

    [HttpGet("authorizedrequest")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult> AuthorizedRequest()
    {
        return Ok();
    }

    [HttpGet("createcase4")]
    public async Task<ActionResult<CaseContextDto>> DoStuff()
    {
        List<CaseDto> s2 = new List<CaseDto>();

        CaseContextDto context = new CaseContextDto()
        {
            Cases = s2,
        };

        return Ok(context);
    }
}
