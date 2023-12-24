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
    public async Task<ActionResult> NotifyPdf(IFormFile pdf, Guid caseId, string documentName) // should 
    {
        var filePath = await _notificationService.SendNotificationWithPdfOnly(pdf, caseId, documentName);
        var virtualFile = File(filePath, "application/octet-stream");
        return virtualFile;
    }
}
