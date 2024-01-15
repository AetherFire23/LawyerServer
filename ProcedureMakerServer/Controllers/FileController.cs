using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Extensions;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository;

namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
	private readonly LawyerRepository _lawyerRepository;
	public FileController(ILogger<FileController> logger,
		LawyerRepository lawyerRepository)
	{
		_lawyerRepository = lawyerRepository;
	}


	// 'formFile' should match the parameter name in the controller action
	[HttpPost("uploadFile")]
	public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
	{
		_ = Request;
		await Task.Delay(2000);
		bool isValidForm = file is not null && file.Length > 0;
		if (!isValidForm) return BadRequest("Invalid file or no file was provided.");

		await file.CreateFileTo($"{"models"}{file.FileName}");

		Console.WriteLine("upkloaded");
		return Ok("File uploaded successfully!");
	}
}
