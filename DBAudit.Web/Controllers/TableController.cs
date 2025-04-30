using DBAudit.Infrastructure.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DBAudit.Web.Controllers;

public class TableController : Controller
{
    private readonly ITableService _tableService;

    public TableController(ITableService tableService)
    {
        _tableService = tableService;
    }

    [HttpGet("/tables/{databaseId:guid}")]
    public IActionResult Index([FromRoute] Guid databaseId)
    {
        var tables = _tableService.GetAll(databaseId);
        return View(tables);
    }
}