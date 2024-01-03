using Microsoft.AspNetCore.Mvc;
using ProcedureMakerServer.Services;
namespace ProcedureMakerServer.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : Controller
{
	private readonly NotificationService _notificationService;

	public NotificationController(NotificationService notificationService)
	{
		_notificationService = notificationService;
	}

	[HttpPost("NotifyPdf")]
	[ProducesResponseType(typeof(FileContentResult), 200)]
	public async Task<FileResult> NotifyPdf(IFormFile pdf, Guid caseId, string documentName) // should 
	{
		var filePath = await _notificationService.SendNotificationWithPdfOnly(pdf, caseId, documentName);
		var fileBytes = System.IO.File.ReadAllBytes(filePath);
		var file = File(fileBytes, "application/pdf");
		return file;
	}
}
