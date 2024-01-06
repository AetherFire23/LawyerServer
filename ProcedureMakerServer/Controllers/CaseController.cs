using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using ProcedureMakerServer.Models;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Services;
using ProcedureShared.Dtos;
using System.Security.Claims;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CaseController : Controller
{
    private Guid InferLawyerId()
    {
        var userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;

        return lawyerId;
    }

    private readonly CaseContextService _caseContextService;
    private readonly LawyerRepository _lawyerRepository;
    private readonly ProcedureContext _procedureContext;
    private readonly ClientRepository _clientRepository;
    private readonly CaseContextRepository _casesContextRepository;
    public CaseController(CaseContextService caseContextService,
                          LawyerRepository lawyerRepository,
                          ProcedureContext procedureContext,
                          ClientRepository clientRepository,
                          CaseContextRepository casesContextRepository)
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
    [HttpGet("GetCasesContext")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult<CaseContextDto>> GetCaseContext()
    {
        var userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;

        if (lawyerId.Equals(Guid.Empty)) throw new ArgumentInvalidException("lawyer id null");
        var caseContext = await _caseContextService.GetCaseContext(lawyerId);
        Console.WriteLine("case hit");
        return Ok(caseContext);
    }

    [HttpPost("CreateNewCase")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult<GetCaseResponse>> CreateNewCase([FromQuery] Guid clientId)
    {
        var userId = new Guid(HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var lawyerId = _procedureContext.Lawyers.First(x => x.UserId == userId).Id;
        var createdId = await _caseContextService.CreateNewCase(lawyerId, clientId);
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
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult<Guid>> AddClient()
    {
        var id = await _clientRepository.CreateClient(this.InferLawyerId());
        return Ok(id);
    }

    [HttpPut("updateclient")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult> UpdateClient([FromBody] ClientDto clientDto)
    {
        await _clientRepository.UpdateClientInfo(clientDto);
        return Ok();
    }

    [HttpGet("CreateCaseParticipant")]
    [Authorize(Roles = nameof(RoleTypes.Normal))]
    public async Task<ActionResult<Guid>> CreateCaseParticipant([FromQuery] Guid caseId)
    {
        var createdId = await _casesContextRepository.AddCaseParticipant(caseId);
        return Ok(createdId);
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
}
