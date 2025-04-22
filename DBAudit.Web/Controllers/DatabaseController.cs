using DBAudit.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DBAudit.Web.Controllers;

public class DatabaseController(IDatabaseService databaseService) : Controller
{
    private readonly IDatabaseService _databaseService = databaseService;

    // GET
    [HttpGet("/databases/{id:guid}")]
    public IActionResult Index([FromRoute] Guid id)
    {
        var databases = _databaseService.GetAll(id);

        return View(databases);
    }
}