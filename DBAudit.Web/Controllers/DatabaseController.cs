using DBAudit.Infrastructure;
using DBAudit.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBAudit.Web.Controllers;

public class DatabaseController(IDatabaseService databaseService, IQueueProvider queueProvider) : Controller
{
    // GET
    [HttpGet("/databases/{id:guid}")]
    public IActionResult Index([FromRoute] Guid id)
    {
        var databases = databaseService.GetAll(id);
        ViewBag.EnvironmentId = id;
        return View(databases);
    }

    [HttpPost("/databases/{envId:guid}/refresh")]
    public IActionResult Refresh([FromRoute] Guid envId)
    {
        queueProvider.Enqueue(new EnvironmentMessage(envId));
        return RedirectToAction(nameof(Index), new { id = envId });
    }
}