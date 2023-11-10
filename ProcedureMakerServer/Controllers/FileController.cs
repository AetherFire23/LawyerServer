using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Extensions;
using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;
    private readonly ILawyerRepository _lawyerRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ICaseRepository _caseRepository;

    public FileController(ILogger<FileController> logger,
        ILawyerRepository lawyerRepository)
    {
        _logger = logger;
        _lawyerRepository = lawyerRepository;
    }

    [HttpPost("addlawyer")]
    public async Task AddLawyer([FromBody] Lawyer lawyer)
    {
        await _lawyerRepository.Create(lawyer);
    }

    [HttpPut("modlawyer")]
    public async Task<IActionResult> ModifyLawyer([FromBody] Lawyer lawyer)
    {
        //var lolzida = new Lawyer();
        //string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(lawyer);
        await _lawyerRepository.ModifyLawyer(lawyer);
        // _lawyerRepository
        return Ok();
    }

    [HttpPost("addclient")]
    public async Task<IActionResult> AddClient([FromBody] Client client) // create and init locally
    {
        await _clientRepository.Create(client);
        return Ok();
    }

    [HttpPut("modclient")]
    public async Task<IActionResult> ModifyClient([FromBody] Client client)
    {
        await _clientRepository.ModifyClient(client);
        return Ok();
    }

    // 'formFile' should match the parameter name in the controller action
    [HttpPost("uploadFile")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        var s = Request;
        await Task.Delay(2000);
        bool isValidForm = file is not null && file.Length > 0;
        if (!isValidForm) return BadRequest("Invalid file or no file was provided.");

        await file.CreateFileTo($"{"models"}{file.FileName}");


        
        Console.WriteLine("upkloaded");
        return Ok("File uploaded successfully!");
    }


}
